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
using System.Threading.Tasks;

namespace Drama.Shard.Interfaces.Maps
{
	/// <summary>
	/// MapDefinitions define static properties of map instances.
	/// 
	/// The key for this grain is the MapId.
	/// </summary>
	public interface IMapDefinition : IGrainWithIntegerKey
	{
		/// <summary>
		/// Returns true if this MapDefinition is defined.
		/// </summary>
		Task<bool> Exists();

		/// <summary>
		/// Gets a MapDefinitionEntity describing this map.
		/// </summary>
		/// <returns></returns>
		Task<MapDefinitionEntity> GetEntity();

		/// <summary>
		/// Merges all non-default values of input into this MapDefinition.
		/// </summary>
		Task Merge(MapDefinitionEntity input);

		/// <summary>
		/// Clears all fields from this MapDefinition and removes it from storage.
		/// </summary>
		/// <returns></returns>
		Task Clear();
	}
}
