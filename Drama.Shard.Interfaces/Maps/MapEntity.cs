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

namespace Drama.Shard.Interfaces.Maps
{
	/// <summary>
	/// Persisted storage for Map grains.
	/// </summary>
	public class MapEntity
	{
		/// <summary>
		/// True if this Map instance has been created.
		/// </summary>
		public bool Exists { get; set; }

		/// <summary>
		/// Gets the map ID of this Map instance. This is the key of the
		/// MapDefinition used to create it.
		/// </summary>
		public int MapId { get; set; }

		/// <summary>
		/// Gets the time when this Map instance was created.
		/// </summary>
		public DateTime CreatedTime { get; set; }

		// TODO: weather stuff
	}
}
