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

using Drama.Shard.Interfaces.Objects;
using Orleans;
using Orleans.Concurrency;
using System.Threading.Tasks;

namespace Drama.Shard.Interfaces.Units
{
	/// <summary>
	/// A Unit grain represents a persistent object that can move.
	/// 
	/// The key for this grain is the ObjectID of the Unit.
	/// </summary>
	public interface IUnit : IUnit<UnitEntity>, IGrainWithIntegerKey
	{

	}

	public interface IUnit<out TEntity> : IPersistentObject<TEntity>, IGrainWithIntegerKey
		where TEntity : UnitEntity, new()
	{
		/// <summary>
		/// Sets the movement state of this Unit. This method is one-way.
		/// </summary>
		[OneWay]
		Task SetMovementState(MovementFlags movementFlags, int time, int fallTime, Jump jump);

		/// <summary>
		/// Broadcasts to subscribed Characters that this Unit has jumped. This
		/// method is one-way.
		/// </summary>
		[OneWay]
		Task Jump();
		
		/// <summary>
		/// Sets this Unit's target to another Unit by id. This method is one-way.
		/// </summary>
		/// <param name="unitId">ObjectID of the object to target. Must be a Unit</param>
		[OneWay]
		Task SetTarget(ObjectID unitId);

		/// <summary>
		/// Clears this Unit's target. This method is one-way.
		/// </summary>
		[OneWay]
		Task ClearTarget();
	}
}
