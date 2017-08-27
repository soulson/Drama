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

using Orleans;
using Orleans.Concurrency;
using System.Threading.Tasks;

namespace Drama.Shard.Interfaces.Objects
{
	/// <summary>
	/// Objects are the root grain for server-side persistent objects. Items,
	/// players, pets, creatures, and gameobjects are all subclasses of Object.
	/// 
	/// The key for this grain is the ObjectID of the object.
	/// </summary>
	public interface IObject : IObject<ObjectEntity>, IGrainWithIntegerKey
	{

	}

	public interface IObject<out TEntity> : IGrainWithIntegerKey
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
	}
}
