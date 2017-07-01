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

namespace Drama.Shard.Interfaces.Objects
{
	[Flags]
	public enum ObjectTypeMask : uint
	{
		None = 0x0000,
		Object = 0x0001,
		Item = 0x0002,
		Container = 0x0004,
		Unit = 0x0008,
		Player = 0x0010,
		GameObject = 0x0020,
		DynamicObject = 0x0040,
		Corpse = 0x0080,
	}
}
