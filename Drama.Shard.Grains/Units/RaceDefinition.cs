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

using Drama.Core.Interfaces;
using Drama.Shard.Interfaces.Units;
using Drama.Shard.Interfaces.Utilities;
using Orleans;
using Orleans.Providers;
using System.Threading.Tasks;

namespace Drama.Shard.Grains.Units
{
	[StorageProvider(ProviderName = StorageProviders.StaticWorld)]
	public class RaceDefinition : Grain<RaceDefinitionEntity>, IRaceDefinition
	{
		public Task<bool> Exists()
			=> Task.FromResult(State.Exists);

		public Task<RaceDefinitionEntity> GetEntity()
		{
			if (!Exists().Result)
				throw new UnitException($"Race {this.GetPrimaryKeyLong()} is not defined");

			return Task.FromResult(State);
		}

		public Task Clear()
		{
			State = new RaceDefinitionEntity();
			return ClearStateAsync();
		}

		public async Task Merge(RaceDefinitionEntity input)
		{
			var entityService = GrainFactory.GetGrain<IEntityService>(0);
			State = await entityService.Merge(State, input);

			State.Exists = true;

			await WriteStateAsync();
		}
	}
}
