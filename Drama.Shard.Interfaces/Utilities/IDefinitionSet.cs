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

using System.Collections.Generic;
using System.Threading.Tasks;

namespace Drama.Shard.Interfaces.Utilities
{
	/// <summary>
	/// Defines the properties of a class of things.
	/// </summary>
	public interface IDefinitionSet<T>
		where T : IDefinitionSetEntry, new()
	{
		/// <summary>
		/// Gets a Set of all definitions in this DefinitionSet.
		/// </summary>
		Task<ISet<T>> GetSet();

		/// <summary>
		/// Clears all definitions from this DefinitionSet.
		/// </summary>
		Task Clear();

		/// <summary>
		/// Adds a new entry to this definition set if the provided entry does not
		/// already exist in this definition set. Does not save to persisted
		/// storage; use Commit for that.
		/// </summary>
		Task AddEntry(T entry);

		/// <summary>
		/// Commits all added entries to persisted storage.
		/// </summary>
		Task Commit();
	}
}
