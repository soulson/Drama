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

using Drama.Auth.Interfaces.Utilities;
using Drama.Core.Interfaces;
using Drama.Shard.Interfaces.Characters;
using Drama.Shard.Interfaces.Maps;
using Orleans;
using Orleans.Providers;
using System.Threading.Tasks;

namespace Drama.Shard.Grains.Maps
{
	[StorageProvider(ProviderName = StorageProviders.DynamicWorld)]
	public class Map : Grain<MapEntity>, IMap
	{
		public Task AddCharacter(CharacterEntity characterEntity)
		{
			VerifyExists();

			return Task.CompletedTask;
		}

		public Task RemoveCharacter(CharacterEntity characterEntity)
		{
			VerifyExists();

			return Task.CompletedTask;
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

		private void VerifyExists()
		{
			if (!Exists().Result)
				throw new MapDoesNotExistException($"{nameof(Map)} instance id {this.GetPrimaryKeyLong()} does not exist");
		}
	}
}
