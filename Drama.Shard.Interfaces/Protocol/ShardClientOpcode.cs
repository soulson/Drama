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

namespace Drama.Shard.Interfaces.Protocol
{
	public enum ShardClientOpcode : uint
	{
		Noop = 0x0000,

		WorldTeleport = 0x0008,
		TeleportToUnit = 0x0009,
		ZoneMap = 0x000a,
		CharacterCreate = 0x0036,
		CharacterList = 0x0037,
		CharacterDelete = 0x0038,
		PlayerLogin = 0x003d,
		PlayerLogout = 0x004a,
		PlayerLogoutRequest = 0x004b,
		PlayerLogoutCancel = 0x004e,
		QueryName = 0x0050,
		QueryPetName = 0x0052,
		QueryGuild = 0x0054,
		QueryItem = 0x0056,
		QueryPageTest = 0x005a,
		QueryQuest = 0x005c,
		QueryGameObject = 0x005e,
		QueryCreature = 0x0060,
		Who = 0x0062,
		WhoIs = 0x0064,
		FriendList = 0x0066,
		FriendAdd = 0x0069,
		FriendDelete = 0x006a,
		ChatMessage = 0x0095,
		ChatJoinChannel = 0x0097,
		ChatLeaveChannel = 0x0098,
		ChatChannelList = 0x009a,
		ChatChannelPassword = 0x009c,
		ChatChannelSetOwner = 0x009d,
		ChatChannelOwner = 0x009e,
		ChatChannelModerator = 0x009f,
		ChatChannelUnmoderator = 0x00a0,
		ChatChannelMute = 0x00a1,
		ChatChannelUnmute = 0x00a2,
		ChatChannelInvite = 0x00a3,
		ChatChannelKick = 0x00a4,
		ChatChannelBan = 0x00a5,
		ChatChannelUnban = 0x00a6,
		ChatChannelAnnouncements = 0x00a7,
		ChatChannelModerate = 0x00a8,
		MoveStartForward = 0x00b5,
		MoveStartBackward = 0x00b6,
		MoveStop = 0x00b7,
		MoveStrafeStartLeft = 0x00b8,
		MoveStrafeStartRight = 0x00b9,
		MoveStrafeStop = 0x00ba,
		MoveJump = 0x00bb,
		MoveTurnStartLeft = 0x00bc,
		MoveTurnStartRight = 0x00bd,
		MoveTurnStop = 0x00be,
		MovePitchStartUp = 0x00bf,
		MovePitchStartDown = 0x00c0,
		MovePitchStop = 0x00c1,
		MoveSetRun = 0x00c2,
		MoveSetWalk = 0x00c3,
		MoveFallLand = 0x00c9,
		MoveSwimStart = 0x00ca,
		MoveSwimStop = 0x00cb,
		MoveSetOrientation = 0x00da,
		MoveSetPitch = 0x00db,
		MoveHeartbeat = 0x00ee,
		TargetSet = 0x013d,
		QueryTime = 0x01ce,
		Ping = 0x01dc,
		AuthSession = 0x01ed,
		ZoneUpdate = 0x01f4,
		AccountDataUpdate = 0x020b,
		SupportTicketQuery = 0x0211,
		SetControlledUnit = 0x026a,
		QueryNextMailTime = 0x284,
		MeetingStoneInfo = 0x0296,
		QueryRaidInfo = 0x02cd,
		MoveTimeSkipped = 0x02ce,
		BattlefieldStatus = 0x02d3,
	}
}
