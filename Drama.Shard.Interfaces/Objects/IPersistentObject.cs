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
using Orleans;
using Orleans.Concurrency;
using System.Threading.Tasks;

namespace Drama.Shard.Interfaces.Objects
{
	/// <summary>
	/// PersistentObjects are the root grain for server-side objects that can be
	/// displayed in the game world.
	/// 
	/// The key for this grain is the ObjectID of the persistent object.
	/// </summary>
	public interface IPersistentObject : IPersistentObject<ObjectEntity>, IGrainWithIntegerKey
	{

	}

	public interface IPersistentObject<out TEntity> : IGrainWithIntegerKey
		where TEntity : ObjectEntity, new()
	{
		/// <summary>
		/// Subscribes an observer to creation, movement, and values updates from
		/// this PersistentObject. This method is one-way.
		/// </summary>
		/// <param name="observer">Cannot be null</param>
		[OneWay]
		Task Subscribe(IObjectObserver observer);

		/// <summary>
		/// Subscribes an observer from creation, movement, and values updates from
		/// this PersistentObject. This method is one-way.
		/// </summary>
		/// <param name="observer">Cannot be null</param>
		[OneWay]
		Task Unsubscribe(IObjectObserver observer);
		
		/// <summary>
		/// Returns true if this PersistentObject has been created.
		/// </summary>
		Task<bool> Exists();

		/// <summary>
		/// Returns true if this object is in-game at this time.
		/// </summary>
		Task<bool> IsIngame();

		/// <summary>
		/// Removes this PersistentObject from the game world.
		/// </summary>
		Task Destroy();

		/// <summary>
		/// Sets the position of this PersistentObject. Does not change the Map
		/// in which the PersistentObject is located. This method is one-way.
		/// </summary>
		[OneWay]
		Task SetPosition(Vector3 position, float orientation);
	}
}
