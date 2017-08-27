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

namespace Drama.Shard.Interfaces.Objects
{
	/// <summary>
	/// This service provides access to PersistentObjects as other types. For
	/// instance, it can provide an IObject view of an ICharacter.
	/// 
	/// The only expected key for this stateless grain is 0.
	/// </summary>
	public interface IObjectService : IGrainWithIntegerKey
	{
		/// <summary>
		/// Gets a reference to any PersistentObject subclass as a PersistentObject
		/// by ObjectId.
		/// </summary>
		Task<IObject<ObjectEntity>> GetObject(ObjectID id);
	}
}
