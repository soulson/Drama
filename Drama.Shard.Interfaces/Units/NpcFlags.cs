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
	/// Specifies which actions an NPC can take for a player.
	/// </summary>
	[Flags]
	public enum NpcFlags : int
	{
		None = 0x00000000,
		Gossip = 0x00000001,
		QuestGiver = 0x00000002,
		Vendor = 0x00000004,
		FlightMaster = 0x00000008,
		Trainer = 0x00000010,
		SpiritHealer = 0x00000020,
		SpiritGuide = 0x00000040,
		Innkeeper = 0x00000080,
		Banker = 0x00000100,
		Petitioner = 0x00000200,
		TabardDesigner = 0x00000400,
		BattleMaster = 0x00000800,
		Auctioneer = 0x00001000,
		StableMaster = 0x00002000,
		Repairman = 0x00004000,
	}
}
