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

namespace Drama.Shard.Interfaces.Chat
{
	/// <summary>
	/// Lists the types of chat messages understood by clients.
	/// </summary>
	public enum ChatMessageType : int
	{
		Addon = -1,
		Say = 0,
		Party = 1,
		Raid = 2,
		Guild = 3,
		Officer = 4,
		Yell = 5,
		Whisper = 6,
		Emote = 8,
		TextEmote = 9,
		System = 10,
		MonsterSay = 11,
		MonsterYell = 12,
		MonsterEmote = 13,
		Channel = 14,
		ChannelJoin = 15,
		ChannelLeave = 16,
		ChannelList = 17,
		Afk = 20,
		Dnd = 21,
		Ignored = 22,
		Skill = 23,
		Loot = 24,
		MonsterWhisper = 26,
	}
}
