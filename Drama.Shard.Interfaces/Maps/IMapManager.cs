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
using Orleans;
using System.Threading.Tasks;

namespace Drama.Shard.Interfaces.Maps
{
	/// <summary>
	/// The MapManager class is responsible for instantiating and disposing of
	/// Map instances. It also provides an interface for interacting with Map
	/// instances, since instance IDs are opaque.
	/// 
	/// It is expected that clients will use the key 0 for this grain.
	/// </summary>
	public interface IMapManager : IGrainWithIntegerKey
	{
		/// <summary>
		/// Gets the instance ID to which a player character is bound. If the
		/// character is not bound to an instance, one will be created for
		/// the character and its ID returned.
		/// 
		/// Instance binding is based on the MapType of the MapId of the character:
		/// 1. If the character exists within a Normal Map, then only one instance
		///    of that Map exists globally, and the character is bound to it.
		/// 2. If the character exists within a Group Map, then the character's
		///    group leader (or the character itself, if not in a group) owns an
		///    instance of that Map and the character is bound to it.
		/// 3. If the character exists in a Raid Map, TBD
		/// 4. If the character exists in a Battleground Map, TBD
		/// </summary>
		/// <returns>An instance ID for the Character</returns>
		Task<long> GetInstanceIdForCharacter(CharacterEntity characterEntity);
	}
}
