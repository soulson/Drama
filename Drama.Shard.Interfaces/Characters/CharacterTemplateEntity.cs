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
using Drama.Shard.Interfaces.Units;

namespace Drama.Shard.Interfaces.Characters
{
	/// <summary>
	/// Persisted storage for CharacterTemplate grains.
	/// </summary>
	public class CharacterTemplateEntity
	{
		public bool Exists { get; set; }
		public Race Race { get; set; }
		public Class Class { get; set; }
		public int MapId { get; set; }
		public int ZoneId { get; set; }
		public Vector3 Position { get; set; }
		public float Orientation { get; set; }
	}
}
