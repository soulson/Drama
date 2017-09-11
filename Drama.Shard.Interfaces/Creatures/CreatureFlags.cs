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

namespace Drama.Shard.Interfaces.Creatures
{
	public enum CreatureFlags : int
	{
		None = 0x00000000,
		Tameable = 0x00000001,
		VisibleToGhosts = 0x00000002,
		HerbalistLoot = 0x00000100,
		MinerLoot = 0x00000200,
		CanAssist = 0x00001000,
		EngineerLoot = 0x00008000,
	}
}
