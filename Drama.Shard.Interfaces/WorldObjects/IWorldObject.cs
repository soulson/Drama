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

using Drama.Core.Interfaces.Numerics;
using Drama.Shard.Interfaces.Objects;
using Orleans;
using Orleans.Concurrency;
using System.Threading.Tasks;

namespace Drama.Shard.Interfaces.WorldObjects
{
	/// <summary>
	/// WorldObjects are Objects that can be displayed in the game world. These
	/// objects have properties like position and orientation.
	/// 
	/// The key for this grain is the ObjectID of the object.
	/// </summary>
	public interface IWorldObject : IWorldObject<WorldObjectEntity>, IGrainWithIntegerKey
	{

	}

	public interface IWorldObject<out TEntity> : IObject<TEntity>, IGrainWithIntegerKey
		where TEntity : WorldObjectEntity, new()
	{

		/// <summary>
		/// Returns true if this object is in-game at this time.
		/// </summary>
		Task<bool> IsIngame();

		/// <summary>
		/// Removes this Object from the game world. This method is one-way.
		/// </summary>
		[OneWay]
		Task Destroy();

		/// <summary>
		/// Sets the position of this Object. Does not change the Map
		/// in which the Object is located. This method is one-way.
		/// </summary>
		[OneWay]
		Task SetPosition(Vector3 position, float orientation);
	}
}
