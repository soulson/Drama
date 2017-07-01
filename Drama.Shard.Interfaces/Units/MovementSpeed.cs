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
	public class MovementSpeed
	{
		public MovementSpeed()
		{
			Walking = 2.5f;
			Running = 7.0f;
			RunningBack = 4.5f;
			Swimming = 4.722222f;
			SwimmingBack = 2.5f;
			Turning = 3.141594f;
		}

		public float Walking { get; set; }
		public float Running { get; set; }
		public float RunningBack { get; set; }
		public float Swimming { get; set; }
		public float SwimmingBack { get; set; }
		public float Turning { get; set; }
	}
}
