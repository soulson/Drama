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

namespace Drama.Shard.Interfaces.Utilities
{
	/// <summary>
	/// Provides stateless services for interacting with Entity objects.
	/// 
	/// It is expected that clients will use the key 0 for this grain.
	/// </summary>
	public interface IEntityService : IGrainWithIntegerKey
	{
		/// <summary>
		/// Merges two entities together. Any property of TEntity with a getter and
		/// setter will be evaluated on @new. If the value of the property is not
		/// the default for the type of the property, then that value is assigned
		/// to the same property on a copy of existing. This copy is then returned.
		/// </summary>
		Task<TEntity> Merge<TEntity>(TEntity existing, TEntity @new);
	}
}
