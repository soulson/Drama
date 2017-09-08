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

using Drama.Shard.Interfaces.Utilities;
using Orleans;
using System.Threading.Tasks;

namespace Drama.Shard.Grains.Utilities
{
	public abstract class AbstractDefinition<T> : Grain<T>, IMergeable<T>
		where T : AbstractDefinitionEntity, new()
	{
		public Task<bool> Exists()
			=> Task.FromResult(State.Exists);

		public Task<T> GetEntity()
		{
			if (!State.Exists)
				throw new DefinitionDoesNotExistException($"template {this.GetPrimaryKeyLong()} of type {this.GetType().Name} does not exist");

			return Task.FromResult(State);
		}

		public Task Clear()
		{
			State = new T();
			return ClearStateAsync();
		}

		public async Task Merge(T input)
		{
			var entityService = GrainFactory.GetGrain<IEntityService>(0);
			State = await entityService.Merge(State, input);

			State.Exists = true;

			await WriteStateAsync();
		}
	}
}
