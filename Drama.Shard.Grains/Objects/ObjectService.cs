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

using Drama.Shard.Interfaces.Characters;
using Drama.Shard.Interfaces.Creatures;
using Drama.Shard.Interfaces.Objects;
using Orleans;
using Orleans.Concurrency;
using System;
using System.Threading.Tasks;

namespace Drama.Shard.Grains.Objects
{
	[StatelessWorker]
	public class ObjectService : Grain, IObjectService
	{
		public Task<IObject<ObjectEntity>> GetObject(ObjectID id)
		{
			switch (id.ObjectType)
			{
				case ObjectID.Type.Player:
					var character = GrainFactory.GetGrain<ICharacter>(id);
					return Task.FromResult<IObject<ObjectEntity>>(character);

				case ObjectID.Type.Unit:
					var creature = GrainFactory.GetGrain<ICreature>(id);
					return Task.FromResult<IObject<ObjectEntity>>(creature);

				default:
					return Task.FromException<IObject<ObjectEntity>>(new NotImplementedException($"object type {id.ObjectType} is not yet implemented by {nameof(ObjectService)}.{nameof(GetObject)}"));
			}
		}
	}
}
