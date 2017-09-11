﻿/* 
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
using Drama.Core.Interfaces.Numerics;
using Drama.Shard.Interfaces.Chat;
using Drama.Shard.Interfaces.Creatures;
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
		public override Task OnActivateAsync()
		{
			var invalidatedObjects = new HashSet<WorldObjectEntity>();

			foreach(var @object in State.Objects)
			{
				if (@object.TypeId == ObjectTypeID.Player)
					invalidatedObjects.Add(@object);
			}

			foreach (var @object in invalidatedObjects)
			{
				GetLogger().Debug($"removing invalidated object {@object.Id} from activating {nameof(Map)} {this.GetPrimaryKeyLong()}");
				State.Objects.Remove(@object);
			}

			return base.OnActivateAsync();
		}

		public async Task AddObject(WorldObjectEntity objectEntity)
		{
			VerifyExists();

			State.Objects.Add(objectEntity);

			var objectService = GrainFactory.GetGrain<IObjectService>(0);
			var @object = await objectService.GetObject(objectEntity.Id);

			await @object.Subscribe(this);

			await WriteStateAsync();
		}

		public async Task RemoveObject(WorldObjectEntity objectEntity)
		{
			VerifyExists();

			var objectService = GrainFactory.GetGrain<IObjectService>(0);
			var @object = await objectService.GetObject(objectEntity.Id);

			await @object.Unsubscribe(this);

			State.Objects.Remove(objectEntity);

			await WriteStateAsync();
		}

		public async Task<MapEntity> Create(int mapId)
		{
			var timeService = GrainFactory.GetGrain<ITimeService>(0);

			State.CreatedTime = await timeService.GetNow();
			State.Exists = true;
			State.MapId = mapId;

			await InitializeCreatures();

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

			foreach (var entity in State.Objects)
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

		private async Task InitializeCreatures()
		{
			var creatureSpawns = await GrainFactory.GetGrain<ICreatureSpawnSet>(State.MapId).GetSet();
			var objectIdService = GrainFactory.GetGrain<IObjectIDGenerator>(0);

			float removeMeDistanceSquare = 120.0f * 120.0f;
			var removeMeStartingArea = new Vector3(-8922.057f, -116.406067f, 82.5350342f);

			foreach (var spawn in creatureSpawns)
			{
				var dist = (spawn.Position.X - removeMeStartingArea.X) * (spawn.Position.X - removeMeStartingArea.X)
				 + (spawn.Position.Y - removeMeStartingArea.Y) * (spawn.Position.Y - removeMeStartingArea.Y)
				 + (spawn.Position.Z - removeMeStartingArea.Z) * (spawn.Position.Z - removeMeStartingArea.Z);
				if (dist < removeMeDistanceSquare)
				{
					var creatureDefinition = await GrainFactory.GetGrain<ICreatureDefinition>(spawn.CreatureDefinitionId).GetEntity();
					var newId = await objectIdService.GenerateObjectId(ObjectID.Type.Unit);
					var creature = GrainFactory.GetGrain<ICreature>(newId);

					await creature.Create(creatureDefinition);
					await creature.SetPosition(spawn.Position, spawn.Orientation);

					State.Objects.Add(await creature.GetCreatureEntity());
					await creature.Subscribe(this);
				}
			}
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
					if (State.Objects.Contains(worldObjectEntity))
					{
						GetLogger().Debug($"{nameof(Map)} instance {this.GetPrimaryKeyLong()} observes a movement update of object {objectEntity.Id}");

						// ObjectEntity is equal on ID, so removing and re-adding it updates its other properties
						State.Objects.Remove(worldObjectEntity);
						State.Objects.Add(worldObjectEntity);
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
