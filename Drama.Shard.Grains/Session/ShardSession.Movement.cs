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

using Drama.Shard.Interfaces.Protocol;
using Drama.Shard.Interfaces.Units;
using System.Threading.Tasks;

namespace Drama.Shard.Grains.Session
{
	public partial class ShardSession
	{
		public Task Move(MovementInPacket request)
		{
			VerifyIngame();

			Jump jump = null;
			if (request.Falling != null)
			{
				jump = new Jump()
				{
					Velocity = request.Falling.Velocity,
					SineAngle = request.Falling.SinAngle,
					CosineAngle = request.Falling.CosAngle,
					XYSpeed = request.Falling.XYSpeed,
				};
			}

			return Task.WhenAll(
					ActiveCharacter.SetPosition(request.Position, request.Orientation),
					ActiveCharacter.SetMovementState(request.MovementFlags, request.Time, request.FallTime, jump)
			);
		}
	}
}
