﻿/* 
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

using Drama.Shard.Interfaces.Formats.Dbc;

namespace Drama.Shard.Interfaces.Units
{
	[DbcEntity("ChrRaces.dbc")]
	public class RaceDefinitionEntity
	{
		public bool Exists { get; set; }

		[DbcKey]
		[DbcFieldOffset(0)]
		public int Id { get; set; }

		[DbcFieldOffset(4)]
		public RaceFlags Flags { get; set; }

		[DbcFieldOffset(8)]
		public int FactionId { get; set; }

		[DbcFieldOffset(16)]
		public int MaleDisplayId { get; set; }

		[DbcFieldOffset(20)]
		public int FemaleDisplayId { get; set; }
	}
}
