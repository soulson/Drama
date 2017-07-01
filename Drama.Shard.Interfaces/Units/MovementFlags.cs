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
	[Flags]
	public enum MovementFlags : int
	{
		None = 0x00000000,
		MoveForward = 0x00000001,
		MoveBackward = 0x00000002,
		MoveLeft = 0x00000004,
		MoveRight = 0x00000008,
		TurnLeft = 0x00000010,
		TurnRight = 0x00000020,
		PitchUp = 0x00000040,
		PitchDown = 0x00000080,

		ModeWalking = 0x00000100,
		ModeLevitating = 0x00000400,
		ModeFlying = 0x00000800,
		ModeFalling = 0x00002000,
		ModeFallingFar = 0x00004000,
		ModeSwimming = 0x00200000,
		SplineEnabled = 0x00400000,
		CanFly = 0x00800000,
		ModeFlyingOld = 0x01000000,

		OnTransport = 0x02000000,
		SplineElevation = 0x04000000,
		Root = 0x08000000,
		ModeWaterWalking = 0x10000000,
		SafeFall = 0x20000000,
		ModeHover = 0x40000000,

		MoveMask = MoveForward | MoveBackward | MoveLeft | MoveRight | TurnLeft | TurnRight,
	};
}
