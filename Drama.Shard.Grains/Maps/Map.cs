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
using Drama.Auth.Interfaces.Utilities;
using Drama.Core.Interfaces;
using Drama.Shard.Interfaces.Chat;
using Drama.Shard.Interfaces.Maps;
using Drama.Shard.Interfaces.Objects;
using Drama.Shard.Interfaces.WorldObjects;
using Orleans;
using Orleans.Providers;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Drama.Shard.Grains.Maps
{
	[StorageProvider(ProviderName = StorageProviders.DynamicWorld)]
	public class Map : Grain<MapEntity>, IMap, IObjectObserver
	{
		// contains all objects in the map. this is inefficient and will obviously
		//  need to be replaced with something more performant eventually
		private readonly ISet<WorldObjectEntity> objects = new HashSet<WorldObjectEntity>();

		public async Task AddObject(WorldObjectEntity objectEntity)
		{
			VerifyExists();

			objects.Add(objectEntity);

			var objectService = GrainFactory.GetGrain<IObjectService>(0);
			var persistentObject = await objectService.GetObject(objectEntity.Id);

			await persistentObject.Subscribe(this);
		}

		public async Task RemoveObject(WorldObjectEntity objectEntity)
		{
			VerifyExists();

			var objectService = GrainFactory.GetGrain<IObjectService>(0);
			var persistentObject = await objectService.GetObject(objectEntity.Id);

			await persistentObject.Unsubscribe(this);

			objects.Remove(objectEntity);
		}

		public async Task<MapEntity> Create(int mapId)
		{
			var timeService = GrainFactory.GetGrain<ITimeService>(0);

			State.CreatedTime = await timeService.GetNow();
			State.Exists = true;
			State.MapId = mapId;

			await WriteStateAsync();

			return State;
		}

		public Task<bool> Exists()
			=> Task.FromResult(State.Exists);

		public Task<MapEntity> GetEntity()
		{
			VerifyExists();

			return Task.FromResult(State);
		}

		public Task<IEnumerable<ObjectID>> GetNearbyObjects(WorldObjectEntity objectEntity, float distance)
		{
			VerifyExists();

			var result = new HashSet<ObjectID>();
			var distanceSquared = distance * distance;

			foreach (var entity in objects)
			{
				var xPart = objectEntity.Position.X - entity.Position.X;
				var yPart = objectEntity.Position.Y - entity.Position.Y;

				xPart *= xPart;
				yPart *= yPart;

				if (xPart + yPart < distanceSquared)
				{
					// don't return objectEntity's id
					if (!entity.Equals(objectEntity))
						result.Add(entity.Id);
				}
			}

			return Task.FromResult<IEnumerable<ObjectID>>(result);
		}

		private void VerifyExists()
		{
			if (!Exists().Result)
				throw new MapDoesNotExistException($"{nameof(Map)} instance id {this.GetPrimaryKeyLong()} does not exist");
		}

		public void HandleObjectCreate(ObjectEntity objectEntity, CreationUpdate update)
		{
			GetLogger().Debug($"{nameof(Map)} instance {this.GetPrimaryKeyLong()} observes the creation of object {objectEntity.Id}");
		}

		public void HandleObjectUpdate(ObjectEntity objectEntity, ObjectUpdate update)
		{
			// Maps only care about object updates if they have moved
			if (update.MovementUpdate != null)
			{
				if (objectEntity is WorldObjectEntity worldObjectEntity)
				{
					if (objects.Contains(worldObjectEntity))
					{
						GetLogger().Debug($"{nameof(Map)} instance {this.GetPrimaryKeyLong()} observes a movement update of object {objectEntity.Id}");

						// ObjectEntity is equal on ID, so removing and re-adding it updates its other properties
						objects.Remove(worldObjectEntity);
						objects.Add(worldObjectEntity);
					}
					else
						GetLogger().Warn($"{nameof(Map)} instance {this.GetPrimaryKeyLong()} observes a movement update of object {objectEntity.Id} which is not in working memory");
				}
			}
		}

		public void HandleObjectDestroyed(ObjectEntity objectEntity)
		{
			GetLogger().Debug($"{nameof(Map)} instance {this.GetPrimaryKeyLong()} observes the destruction of object {objectEntity.Id}");
		}

		public void HandleSay(ObjectEntity objectEntity, string message, ChatLanguage language)
		{
			// map doesn't care
		}

		public void HandleYell(ObjectEntity objectEntity, string message, ChatLanguage language)
		{
			// map doesn't care
		}
	}
}
