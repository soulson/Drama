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

using Drama.Core.Interfaces.Numerics;
using Drama.Shard.Interfaces.Objects;

namespace Drama.Shard.Interfaces.WorldObjects
{
	public class WorldObjectEntity : ObjectEntity
	{
		/// <summary>
		/// Creates a new instance of the WorldObjectEntity class.
		/// </summary>
		public WorldObjectEntity() : this((short)ObjectFields.END)
		{
			// persistent object entity public default constructors defer to a protected constructor
		}

		protected WorldObjectEntity(int fieldCount) : base(fieldCount)
		{

		}

		/// <summary>
		/// The location of this object within its map.
		/// </summary>
		public Vector3 Position { get; set; }

		/// <summary>
		/// The direction that this object is facing, in radians.
		/// </summary>
		public float Orientation { get; set; }

		/// <summary>
		/// The id of the map in which this object exists.
		/// </summary>
		public int MapId { get; set; }
	}
}
