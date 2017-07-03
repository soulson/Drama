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

using System;

namespace Drama.Shard.Interfaces.Formats.Dbc
{
	/// <summary>
	/// Apply this attribute to specify the offset of a column within a DBC row.
	/// </summary>
	[AttributeUsage(AttributeTargets.Property, AllowMultiple = false, Inherited = true)]
	public sealed class DbcFieldOffsetAttribute : Attribute
	{
		/// <summary>
		/// Gets the offset of this column within a DBC row.
		/// </summary>
		public int Offset { get; }

		/// <summary>
		/// Creates a new instance of DbcFieldOffsetAttribute.
		/// </summary>
		/// <param name="offset">
		/// The offset of this column within a DBC row.
		/// Must be greater than or equal to zero</param>
		public DbcFieldOffsetAttribute(int offset)
		{
			if (offset < 0)
				throw new ArgumentException("must be greater or equal to zero", nameof(offset));

			Offset = offset;
		}
	}
}
