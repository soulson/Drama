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
using Drama.Shard.Interfaces.Characters;
using Drama.Shard.Interfaces.Maps;
using Orleans;
using Orleans.Providers;
using System;
using System.Threading.Tasks;

namespace Drama.Shard.Grains.Maps
{
	[StorageProvider(ProviderName = StorageProviders.DynamicWorld)]
	public class MapManager : Grain<MapManagerEntity>, IMapManager
	{
		public async Task<long> GetInstanceIdForCharacter(CharacterEntity characterEntity)
		{
			var mapDefinition = GrainFactory.GetGrain<IMapDefinition>(characterEntity.MapId);
			var mapDefinitionEntity = await mapDefinition.GetEntity();

			long instanceId;
			switch (mapDefinitionEntity.Type)
			{
				case MapType.Normal:
					// one instance of Map per shard
					instanceId = await GetOrCreateNormalInstanceAsync(characterEntity.MapId);
					break;
				default:
					// not implemented MapType
					throw new NotImplementedException($"{nameof(MapType)}.{mapDefinitionEntity.Type} is not yet implemented");
			}

			await WriteStateAsync();

			return instanceId;
		}

		/// <remarks>
		/// Normal Maps are global and have an indefinite lifespan, so there's no
		/// fancy or complicated logic to deal with them.
		/// 
		/// This method does not check to ensure that mapId is actually a Normal
		/// map.
		/// 
		/// Remember to write state after using this method.
		/// </remarks>
		private async Task<long> GetOrCreateNormalInstanceAsync(int mapId)
		{
			if (!State.NormalInstanceIds.ContainsKey(mapId))
				State.NormalInstanceIds.Add(mapId, GetNextInstanceId());

			var instanceId = State.NormalInstanceIds[mapId];
			var instance = GrainFactory.GetGrain<IMap>(instanceId);

			// as long as no other Grain types try to create new Map instances, then
			//  this cannot cause a race condition. this does, however, prevent this
			//  grain from becoming reentrant
			if (!await instance.Exists())
			{
				var entity = await instance.Create(mapId);
				GetLogger().Info($"created new normal instance of map {entity.MapId} with ID {instanceId}");
			}

			return instanceId;
		}

		/// <remarks>
		/// Remember to write state after using this method.
		/// </remarks>
		private long GetNextInstanceId()
			=> State.NextInstanceId++;
	}
}
