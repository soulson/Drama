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

namespace Drama.Shard.Interfaces.Units
{
	/// <summary>
	/// Enumerates the possible stand states for a unit (standing, sitting, kneeling, etc.)
	/// </summary>
	/// <remarks>
	/// UnitFields.Bytes1[0]
	/// </remarks>
	public enum StandState : byte
	{
		Stand = 0,
		Sit = 1,
		SitChair = 2,
		Sleep = 3,
		SitLowChair = 4,
		SitMediumChair = 5,
		SitHighChair = 6,
		Dead = 7,
		Kneel = 8,
	}
}
