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
	public enum ShardServerOpcode : ushort
	{
		Noop = 0x0000,

		ZoneMap = 0x000b,
		CharacterCreate = 0x003a,
		CharacterList = 0x003b,
		CharacterDelete = 0x003c,
		NewWorld = 0x003e,
		TransferPending = 0x003f,
		TransferAborted = 0x0040,
		PlayerLoginFailed = 0x0041,
		LoginSetTimeAndSpeed = 0x0042,
		PlayerLogoutResponse = 0x004c,
		PlayerLogoutComplete = 0x004d,
		PlayerLogoutCancel = 0x004f,
		QueryName = 0x0051,
		QueryPetName = 0x0053,
		QueryGuild = 0x0055,
		QueryItem = 0x0058,
		QueryPageText = 0x005b,
		QueryQuest = 0x005d,
		QueryGameObject = 0x005f,
		QueryCreature = 0x0061,
		Who = 0x0063,
		WhoIs = 0x0065,
		FriendList = 0x0067,
		FriendStatus = 0x0068,
		IgnoreList = 0x006b,
		ChatMessage = 0x0096,
		ChatChannelNotify = 0x0099,
		ChatChannelList = 0x009b,
		ObjectUpdate = 0x00a9,
		ObjectDestroy = 0x00aa,
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
		TriggerCinematic = 0x00fa,
		LoginTutorialFlags = 0x00fd,
		LoginInitializeFactions = 0x0122,
		SetProficiency = 0x0127,
		LoginInitializeActionButtons = 0x0129,
		LoginInitializeSpells = 0x012a,
		SpellCastFailed = 0x0130,
		SpellStart = 0x0131,
		SpellGo = 0x0132,
		SpellFailure = 0x0133,
		SpellCooldown = 0x0134,
		BindPointUpdate = 0x0155,
		QueryTime = 0x01cf,
		Pong = 0x01dd,
		AuthChallenge = 0x01ec,
		AuthResponse = 0x01ee,
		ObjectUpdateCompressed = 0x01f6,
		AccountDataTimes = 0x0209,
		SupportTicketQuery = 0x0212,
		LoginSetRestStart = 0x021e,
		LoginVerifyWorld = 0x0236,
		QueryNextMailTime = 0x0284,
		MeetingStoneSetQueue = 0x0295,
		InitializeWorldState = 0x02c2,
		QueryRaidInfo = 0x02cc,
		AddonInfo = 0x02ef,
		TargetClear = 0x03be,
	}
}
