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
	/// Lists the chat languages.
	/// </summary>
	public enum ChatLanguage : int
	{
		Addon = -1,
		Universal = 0,
		Orcish = 1,
		Darnassian = 2,
		Taurahe = 3,
		Dwarvish = 6,
		Common = 7,
		Demonic = 8,
		Titan = 9,
		Thalassian = 10,
		Draconic = 11,
		Kalimag = 12,
		Gnomish = 13,
		Troll = 14,
		Gutterspeak = 33,
	}
}
