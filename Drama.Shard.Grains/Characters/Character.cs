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
using System.Threading.Tasks;

namespace Drama.Shard.Grains.Characters
{
	[StorageProvider(ProviderName = StorageProviders.DynamicWorld)]
	public sealed class Character : AbstractCharacter<CharacterEntity>, ICharacter
	{

	}

	public abstract class AbstractCharacter<TEntity> : AbstractUnit<TEntity>, ICharacter<TEntity>, IObjectObserver
		where TEntity : CharacterEntity, new()
	{
		protected Guid SessionId { get; private set; } = Guid.Empty;

		public AbstractCharacter()
		{
			UpdatePeriodSeconds = 1.0 / 20.0;
		}

		public async Task<CharacterEntity> Create(string name, string account, string shard, Race race, Class @class, Sex sex, byte skin, byte face, byte hairStyle, byte hairColor, byte facialHair)
		{
			if (IsExists)
				throw new CharacterAlreadyExistsException($"{GetType().Name} with objectid {State.Id} already exists");

			var raceDefinitionTask = GrainFactory.GetGrain<IRaceDefinition>((byte)race).GetEntity();
			var characterTemplate = await GrainFactory.GetGrain<ICharacterTemplate>(((byte)race << 8) + (byte)@class).GetEntity();

			State.Account = account;
			State.Class = characterTemplate.Class;
			State.Enabled = true;
			State.Face = face;
			State.FacialHair = facialHair;
			State.HairColor = hairColor;
			State.HairStyle = hairStyle;
			State.Id = new ObjectID(this.GetPrimaryKeyLong());
			State.Level = 1;
			State.MapId = characterTemplate.MapId;
			State.Name = name;
			State.Orientation = characterTemplate.Orientation;
			State.Position = characterTemplate.Position;
			State.Race = characterTemplate.Race;
			State.Sex = sex;
			State.Shard = shard;
			State.Skin = skin;
			State.ZoneId = characterTemplate.ZoneId;

			var raceDefinition = await raceDefinitionTask;
			
			State.FactionTemplate = raceDefinition.FactionId;
			if (sex == Sex.Male)
			{
				State.DisplayID = raceDefinition.MaleDisplayId;
				State.NativeDisplayID = raceDefinition.MaleDisplayId;
			}
			else if (sex == Sex.Female)
			{
				State.DisplayID = raceDefinition.FemaleDisplayId;
				State.NativeDisplayID = raceDefinition.FemaleDisplayId;
			}
			else
				throw new CharacterException($"invalid {nameof(Character)} {nameof(Sex)} {sex}");

			await WriteStateAsync();

			return State;
		}

		public Task<Guid> GetSessionId()
		{
			VerifyOnline();

			return Task.FromResult(SessionId);
		}

		public Task<CharacterEntity> GetCharacterEntity()
		{
			VerifyExists();

			return Task.FromResult<CharacterEntity>(State);
		}

		public async Task Login(Guid sessionId)
		{
			VerifyExists();

			if (sessionId == Guid.Empty)
				throw new ArgumentException($"cannot be an empty {nameof(Guid)}", nameof(sessionId));

			SessionId = sessionId;

			await ActivateWorkingSet();

			// send creation update for self to client
			var objectUpdateRequest = new ObjectUpdateRequest()
			{
				TargetObjectId = State.Id,
			};
			objectUpdateRequest.ObjectUpdates.Add(GetCreationUpdate());

			var mapManager = GrainFactory.GetGrain<IMapManager>(0);
			var mapInstanceId = await mapManager.GetInstanceIdForCharacter(State);
			var mapInstance = GrainFactory.GetGrain<IMap>(mapInstanceId);
			await mapInstance.AddObject(State);

			await Send(objectUpdateRequest);
		}

		public async Task Logout()
		{
			VerifyOnline();

			await DeactivateWorkingSet();

			var mapManager = GrainFactory.GetGrain<IMapManager>(0);
			var mapInstanceId = await mapManager.GetInstanceIdForCharacter(State);
			var mapInstance = GrainFactory.GetGrain<IMap>(mapInstanceId);
			await mapInstance.RemoveObject(State);

			SessionId = Guid.Empty;

			await Destroy();
		}

		public Task Send(IOutPacket message)
		{
			if (IsOnline().Result)
			{
				var session = GrainFactory.GetGrain<IShardSession>(GetSessionId().Result);
				session.Send(message);
			}
			else
				GetLogger().Warn($"tried to send packet of type {message.GetType().Name} to not-online {nameof(Character)} {State.Name}");

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

		/// <summary>
		/// Gets the Map instance that this Character is currently in.
		/// </summary>
		protected override async Task<IMap> GetMapInstance()
		{
			var mapManager = GrainFactory.GetGrain<IMapManager>(0);
			var mapInstanceId = await mapManager.GetInstanceIdForCharacter(State);
			return GrainFactory.GetGrain<IMap>(mapInstanceId);
		}

		public override void HandleObjectCreate(ObjectEntity objectEntity, CreationUpdate update)
		{
			base.HandleObjectCreate(objectEntity, update);

			HandleObjectUpdate(objectEntity, update);
		}

		public override void HandleObjectUpdate(ObjectEntity objectEntity, ObjectUpdate update)
		{
			base.HandleObjectUpdate(objectEntity, update);

			var tasks = new LinkedList<Task>();

			var objectUpdateRequest = new ObjectUpdateRequest()
			{
				TargetObjectId = State.Id,
			};
			objectUpdateRequest.ObjectUpdates.Add(update);
			tasks.AddLast(Send(objectUpdateRequest));

			// for Unit entities, we need to send move start/stop/heartbeat packets as well
			if (objectEntity is UnitEntity unitEntity)
			{
				var opcodes = new LinkedList<ShardServerOpcode>();

				if ((unitEntity.MoveFlags & MovementFlags.MoveMask) != 0 && unitEntity.PreviousMoveFlags == unitEntity.MoveFlags)
				{
					// heartbeat
					opcodes.AddLast(ShardServerOpcode.MoveHeartbeat);
				}
				else
				{
					// complicated
					if ((unitEntity.MoveFlags & MovementFlags.MoveLeft) != 0 && (unitEntity.PreviousMoveFlags & MovementFlags.MoveLeft) == 0)
						opcodes.AddLast(ShardServerOpcode.MoveStrafeStartLeft);
					else if ((unitEntity.MoveFlags & MovementFlags.MoveRight) != 0 && (unitEntity.PreviousMoveFlags & MovementFlags.MoveRight) == 0)
						opcodes.AddLast(ShardServerOpcode.MoveStrafeStartRight);
					else if ((unitEntity.MoveFlags & (MovementFlags.MoveLeft | MovementFlags.MoveRight)) == 0 && (unitEntity.PreviousMoveFlags & (MovementFlags.MoveLeft | MovementFlags.MoveRight)) != 0)
						opcodes.AddLast(ShardServerOpcode.MoveStrafeStop);

					if ((unitEntity.MoveFlags & MovementFlags.MoveForward) != 0 && (unitEntity.PreviousMoveFlags & MovementFlags.MoveForward) == 0)
						opcodes.AddLast(ShardServerOpcode.MoveStartForward);
					else if ((unitEntity.MoveFlags & MovementFlags.MoveBackward) != 0 && (unitEntity.PreviousMoveFlags & MovementFlags.MoveBackward) == 0)
						opcodes.AddLast(ShardServerOpcode.MoveStartBackward);
					else if ((unitEntity.MoveFlags & (MovementFlags.MoveForward | MovementFlags.MoveBackward)) == 0 && (unitEntity.PreviousMoveFlags & (MovementFlags.MoveForward | MovementFlags.MoveBackward)) != 0)
						opcodes.AddLast(ShardServerOpcode.MoveStop);

					if ((unitEntity.MoveFlags & MovementFlags.TurnLeft) != 0 && (unitEntity.PreviousMoveFlags & MovementFlags.TurnLeft) == 0)
						opcodes.AddLast(ShardServerOpcode.MoveTurnStartLeft);
					else if ((unitEntity.MoveFlags & MovementFlags.TurnRight) != 0 && (unitEntity.PreviousMoveFlags & MovementFlags.TurnRight) == 0)
						opcodes.AddLast(ShardServerOpcode.MoveTurnStartRight);
					else if ((unitEntity.MoveFlags & (MovementFlags.TurnLeft | MovementFlags.TurnRight)) == 0 && (unitEntity.PreviousMoveFlags & (MovementFlags.TurnLeft | MovementFlags.TurnRight)) != 0)
						opcodes.AddLast(ShardServerOpcode.MoveTurnStop);
				}

				if (unitEntity.Orientation != unitEntity.PreviousOrientation && (unitEntity.MoveFlags & (MovementFlags.TurnLeft | MovementFlags.TurnRight)) == 0)
					opcodes.AddLast(ShardServerOpcode.MoveSetOrientation);

				foreach(var opcode in opcodes)
				{
					var movePacket = new MovementOutPacket(opcode)
					{
						ObjectId = unitEntity.Id,

						FallTime = unitEntity.FallTime,
						MovementFlags = unitEntity.MoveFlags,
						Orientation = unitEntity.Orientation,
						Pitch = unitEntity.MovePitch,
						Position = unitEntity.Position,
						Time = unitEntity.MoveTime,

						Falling = new MovementOutPacket.FallingInfo()
						{
							CosAngle = unitEntity.Jump.CosineAngle,
							SinAngle = unitEntity.Jump.SineAngle,
							Velocity = unitEntity.Jump.Velocity,
							XYSpeed = unitEntity.Jump.XYSpeed,
						},
						Transport = null,
					};

					tasks.AddLast(Send(movePacket));
				}
			}

			Task.WhenAll(tasks).Wait();
		}

		public override void HandleObjectDestroyed(ObjectEntity objectEntity)
		{
			base.HandleObjectDestroyed(objectEntity);

			var objectDestroyRequest = new ObjectDestroyRequest()
			{
				ObjectId = objectEntity.Id,
			};

			Send(objectDestroyRequest).Wait();
		}
	}
}
