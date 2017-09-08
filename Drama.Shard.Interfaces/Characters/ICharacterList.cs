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
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Drama.Shard.Interfaces.Characters
{
	/// <summary>
	/// The CharacterList grain maintains a list of all Characters on a shard.
	/// 
	/// The key for this grain is the name of the shard for which it is listing.
	/// </summary>
	public interface ICharacterList : IGrainWithStringKey
	{
		/// <summary>
		/// Gets a list of ObjectIDs that represent the Characters owneed by accountName.
		/// </summary>
		Task<IList<ObjectID>> GetCharactersByAccount(string accountName);

		/// <summary>
		/// Gets the ObjectID of the Character whose name is characterName, if it
		/// exists. Returns null if it does not.
		/// </summary>
		Task<ObjectID?> GetCharacterByName(string characterName);

		/// <summary>
		/// Acquires a new ObjectID for a Character and relates it with the given
		/// characterName and accountName.
		/// </summary>
		/// <returns>An ObjectID that can be used to create a new Character</returns>
		Task<ObjectID> AddCharacter(string characterName, string accountName);

		/// <summary>
		/// Removes a Character from the CharacterList by character name and
		/// account name.
		/// </summary>
		Task RemoveCharacter(string characterName, string accountName);
	}
}
