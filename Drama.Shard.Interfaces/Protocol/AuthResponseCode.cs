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
	public enum AuthResponseCode : byte
	{
		Success = 0x0C,
		Failed = 0x0D,
		Rejected = 0x0E,
		BadServerProof = 0x0F,
		Unavailable = 0x10,
		SystemError = 0x11,
		BillingError = 0x12,
		BillingExpired = 0x13,
		VersionMismatch = 0x14,
		UnknownAccount = 0x15,
		BadCredentials = 0x16,
		SessionExpired = 0x17,
		ServerShuttingDown = 0x18,
		AlreadyLoggingIn = 0x19,
		LoginServerNotFound = 0x1A,
		WaitQueue = 0x1B,
		Banned = 0x1C,
		AlreadyOnline = 0x1D,
		NoTimeRemaining = 0x1E,
		DatabaseBusy = 0x1F,
		Suspended = 0x20,
		ParentalControl = 0x21,
	}
}
