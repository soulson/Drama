/* 
 * The Drama project: what you get when a bunch of actors try to host a game.
 * Copyright (C) 2017 Soulson
 *
 * This program is free software: you can redistribute it and/or modify
 * it under the terms of the GNU Affero General Public License as
 * published by the Free Software Foundation, either version 3 of the
 * License, or (at your option) any later version.
 *
 * This program is distributed in the hope that it will be useful,
 * but WITHOUT ANY WARRANTY; without even the implied warranty of
 * MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
 * GNU Affero General Public License for more details.
 *
 * You should have received a copy of the GNU Affero General Public License
 * along with this program.  If not, see <http://www.gnu.org/licenses/>.
 */

using Drama.Auth.Interfaces;
using Drama.Core.Interfaces;
using Drama.Core.Interfaces.Networking;
using Drama.Core.Interfaces.Numerics;
using Drama.Shard.Grains.Units;
using Drama.Shard.Interfaces.Characters;
using Drama.Shard.Interfaces.Maps;
using Drama.Shard.Interfaces.Objects;
using Drama.Shard.Interfaces.Protocol;
using Drama.Shard.Interfaces.Session;
using Drama.Shard.Interfaces.Units;
using Orleans;
using Orleans.Providers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Drama.Shard.Grains.Characters
{
	[StorageProvider(ProviderName = StorageProviders.DynamicWorld)]
	public sealed class Character : AbstractCharacter<CharacterEntity>, ICharacter
	{

	}

	public abstract class AbstractCharacter<TEntity> : AbstractUnit<TEntity>, ICharacter<TEntity>
		where TEntity : CharacterEntity, new()
	{
		// TODO: make this a configuration item
		/// <summary>
		/// Characters will update which PersistentObjects are in its working set
		/// with the frequency established by this value.
		/// </summary>
		private const double WorkingSetUpdatePeriodSeconds = 1.0;

		// TODO: this should be like, anything but a const float in AbstractCharacter
		private const float ViewDistance = 60.0f;

		private IDisposable workingSetUpdateTimerHandle;
		private readonly ISet<ObjectID> workingSet = new HashSet<ObjectID>();

		protected Guid SessionId { get; private set; } = Guid.Empty;

		public override Task OnDeactivateAsync()
		{
			workingSetUpdateTimerHandle?.Dispose();
			workingSetUpdateTimerHandle = null;

			workingSet.Clear();

			return base.OnDeactivateAsync();
		}

		public async Task<TEntity> Create(string name, string account, string shard, Race race, Class @class, Sex sex, byte skin, byte face, byte hairStyle, byte hairColor, byte facialHair)
		{
			if (IsExists)
				throw new CharacterAlreadyExistsException($"{GetType().Name} with objectid {State.Id} already exists");
			
			State.Account = account;
			State.Class = @class;
			State.Enabled = true;
			State.Face = face;
			State.FacialHair = facialHair;
			State.HairColor = hairColor;
			State.HairStyle = hairStyle;
			State.Id = new ObjectID(this.GetPrimaryKeyLong());
			State.Level = 1;
			State.MapId = 0; // TODO
			State.Name = name;
			State.Orientation = 0.0f; // TODO
			State.Position = new Vector3(-8949.95f, -132.493f, 83.5312f); // TODO
			State.Race = race;
			State.Sex = sex;
			State.Shard = shard;
			State.Skin = skin;
			State.ZoneId = 12; // TODO
			State.FactionTemplate = 1; // TODO
			State.DisplayID = State.NativeDisplayID = 50; // TODO

			await WriteStateAsync();

			return State;
		}

		public Task<Guid> GetSessionId()
		{
			VerifyOnline();

			return Task.FromResult(SessionId);
		}

		public Task Login(Guid sessionId)
		{
			VerifyExists();

			if (sessionId == Guid.Empty)
				throw new ArgumentException($"cannot be an empty {nameof(Guid)}", nameof(sessionId));

			SessionId = sessionId;

			if (workingSetUpdateTimerHandle == null)
				workingSetUpdateTimerHandle = RegisterTimer(UpdateWorkingSet, null, TimeSpan.FromSeconds(WorkingSetUpdatePeriodSeconds), TimeSpan.FromSeconds(WorkingSetUpdatePeriodSeconds));
			else
				GetLogger().Warn($"{nameof(Character)} {State.Name} logged in but {nameof(workingSetUpdateTimerHandle)} was not null");

			return Task.CompletedTask;
		}

		public Task Logout()
		{
			VerifyOnline();

			workingSetUpdateTimerHandle?.Dispose();
			workingSetUpdateTimerHandle = null;

			SessionId = Guid.Empty;

			return Destroy();
		}

		public Task Send(IOutPacket message)
		{
			VerifyOnline();

			var session = GrainFactory.GetGrain<IShardSession>(GetSessionId().Result);
			session.Send(message);

			return Task.CompletedTask;
		}

		protected void VerifyOnline()
		{
			VerifyExists();

			if (!IsOnline().Result)
				throw new CharacterException($"{nameof(Character)} is not ingame");
		}

		public Task<bool> IsOnline()
			=> Task.FromResult(SessionId != Guid.Empty);

		public override Task<bool> IsIngame()
			=> Task.FromResult(base.IsIngame().Result && IsOnline().Result);

		/// <remarks>
		/// This method gets invoked by the update timer every
		/// WorkingSetUpdatePeriodSeconds. It updates the working set of this
		/// Character; that is, which PersistentObjects it can see. The argument is
		/// always null.
		/// </remarks>
		private async Task UpdateWorkingSet(object arg)
		{
			var map = await GetMapInstance();
			var newWorkingSet = await map.GetNearbyObjects(State, ViewDistance);

			var addedObjects = newWorkingSet.Except(workingSet);
			var removedObjects = workingSet.Except(newWorkingSet);

			// is this the most efficient way to set workingSet?
			workingSet.Clear();
			workingSet.UnionWith(newWorkingSet);

			var tasks = new LinkedList<Task>();

			foreach(var removedObject in removedObjects)
			{
				// TODO: fill in with generic GetGrain, then unsubscribe
				var removeRequest = new ObjectDestroyRequest()
				{
					ObjectId = removedObject,
				};

				tasks.AddLast(Send(removeRequest));
			}

			foreach(var addedObject in addedObjects)
			{
				// TODO: fill in with generic GetGrain, then subscribe, getCreationUpdate, and send
			}

			await Task.WhenAll(tasks);
		}

		/// <summary>
		/// Gets the Map instance that this Character is currently in.
		/// </summary>
		protected async Task<IMap> GetMapInstance()
		{
			var mapManager = GrainFactory.GetGrain<IMapManager>(0);
			var mapInstanceId = await mapManager.GetInstanceIdForCharacter(State);
			return GrainFactory.GetGrain<IMap>(mapInstanceId);
		}
	}
}
