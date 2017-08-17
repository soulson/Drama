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
	/// PersistentObjects are the root grain for server-side objects that can be
	/// displayed in the game world.
	/// 
	/// The key for this grain is the ObjectID of the persistent object.
	/// </summary>
	public interface IPersistentObject : IPersistentObject<ObjectEntity>, IGrainWithIntegerKey
	{

	}

	public interface IPersistentObject<TEntity> : IGrainWithIntegerKey
		where TEntity : ObjectEntity, new()
	{
		[OneWay]
		Task Subscribe(IObjectObserver observer);
		[OneWay]
		Task Unsubscribe(IObjectObserver observer);

		Task<bool> Exists();
		Task<TEntity> GetEntity();
		Task<CreationUpdate> GetCreationUpdate();
	}
}
