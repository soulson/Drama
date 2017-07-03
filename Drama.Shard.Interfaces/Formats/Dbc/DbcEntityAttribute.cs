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
	/// Apply this attribute to specify that an entity can be read from a DBC
	/// file.
	/// </summary>
	[AttributeUsage(AttributeTargets.Class | AttributeTargets.Struct, AllowMultiple = false, Inherited = false)]
	public sealed class DbcEntityAttribute : Attribute
	{
		/// <summary>
		/// Gets the file name of the DBC file to load this entity from, including
		/// the extension.
		/// </summary>
		public string DbcFileName { get; }

		/// <summary>
		/// Creates a new instance of DbcEntityAttribute.
		/// </summary>
		/// <param name="dbcFileName">
		/// The name of the DBC file to load from, including the extension.
		/// Cannot be null
		/// </param>
		public DbcEntityAttribute(string dbcFileName)
			=> DbcFileName = dbcFileName ?? throw new ArgumentNullException(nameof(dbcFileName));
	}
}
