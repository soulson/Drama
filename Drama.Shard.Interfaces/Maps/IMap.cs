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
using Drama.Shard.Interfaces.WorldObjects;
using Orleans;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Drama.Shard.Interfaces.Maps
{
	/// <summary>
	/// The Map grain represents an "instance" of a map in the game world. All
	/// map types are represented as Map grains, including non-instanced world
	/// maps and instance-on-demand battleground maps.
	/// 
	/// The key for this grain is the instance ID.
	/// </summary>
	public interface IMap : IGrainWithIntegerKey
	{
		/// <summary>
		/// Creates a new Map instance of the given mapId.
		/// </summary>
		Task<MapEntity> Create(int mapId);

		/// <summary>
		/// Returns true if this Map has been created.
		/// </summary>
		Task<bool> Exists();

		/// <summary>
		/// Gets the entity representing this Map.
		/// </summary>
		Task<MapEntity> GetEntity();

		/// <summary>
		/// Adds a new PersistentObject to this Map instance.
		/// </summary>
		Task AddObject(WorldObjectEntity objectEntity);

		/// <summary>
		/// Removes a PersistentObject from this Map instance.
		/// </summary>
		Task RemoveObject(WorldObjectEntity objectEntity);

		/// <summary>
		/// Returns a collection of ObjectIDs representing objects within a
		/// specified distance of objectEntity.
		/// </summary>
		/// <param name="distance">maximum distance of returned objects, exclusive</param>
		Task<IEnumerable<ObjectID>> GetNearbyObjects(WorldObjectEntity objectEntity, float distance);
	}
}
