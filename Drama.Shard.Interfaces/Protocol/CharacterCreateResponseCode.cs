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
	public enum CharacterCreateResponseCode : byte
	{
		InProgress = 0x2D,
		Success = 0x2E,
		Error = 0x2F,
		Failed = 0x30,
		NameTaken = 0x31,
		Disabled = 0x3A,
		FactionViolation = 0x33,
		ServerLimit = 0x34,
		AccountLimit = 0x35,
	}
}
