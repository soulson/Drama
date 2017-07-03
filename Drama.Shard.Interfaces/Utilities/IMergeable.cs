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

using System.Threading.Tasks;

namespace Drama.Shard.Interfaces.Utilities
{
	/// <summary>
	/// Defines methods for merging static data from different sources into a
	/// single object.
	/// </summary>
	public interface IMergeable<T>
	{
		/// <summary>
		/// Merges all non-default values of input into this IMergeable.
		/// </summary>
		Task Merge(T input);

		/// <summary>
		/// Clears all fields from this IMergeable, resetting it to its default
		/// state.
		/// </summary>
		Task Clear();
	}
}
