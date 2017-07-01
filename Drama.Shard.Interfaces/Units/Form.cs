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
	public enum Form : byte
	{
		None = 0x00,
		Cat = 0x01,
		Tree = 0x02,
		Travel = 0x03,
		Aqua = 0x04,
		Bear = 0x05,
		Ambient = 0x06,
		Ghoul = 0x07,
		DireBear = 0x08,
		CreatureBear = 0x0e,
		CreatureCat = 0x0f,
		GhostWolf = 0x10,
		BattleStance = 0x11,
		DefensiveStance = 0x12,
		BerserkerStance = 0x13,
		Shadow = 0x1c,
		Stealth = 0x1e,
		Moonkin = 0x1f,
		SpiritOfRedemption = 0x20,
	}
}
