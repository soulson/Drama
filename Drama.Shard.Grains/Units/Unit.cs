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

using Drama.Shard.Grains.Objects;
using Drama.Shard.Interfaces.Objects;
using Drama.Shard.Interfaces.Units;
using System.Threading.Tasks;

namespace Drama.Shard.Grains.Units
{
	public sealed class Unit : AbstractUnit<UnitEntity>, IUnit
	{

	}

	public abstract class AbstractUnit<TEntity> : AbstractPersistentObject<TEntity>, IUnit<TEntity>
		where TEntity : UnitEntity, new()
	{
		protected override ObjectUpdateFlags UpdateFlags => base.UpdateFlags | ObjectUpdateFlags.Living;

		protected override MovementUpdate BuildMovementUpdate()
			=> new MovementUpdate(State);

		protected override void OnPostUpdate()
		{
			base.OnPostUpdate();

			State.PreviousMoveFlags = State.MoveFlags;
			State.PreviousOrientation = State.Orientation;
		}

		public Task SetMovementState(MovementFlags movementFlags, int time, int fallTime, Jump jump)
		{
			VerifyExists();

			State.MoveFlags = movementFlags;
			State.MoveTime = time;
			State.FallTime = fallTime;

			if (jump != null)
			{
				State.Jump.Velocity = jump.Velocity;
				State.Jump.SineAngle = jump.SineAngle;
				State.Jump.CosineAngle = jump.CosineAngle;
				State.Jump.XYSpeed = jump.XYSpeed;
			}
			else
				State.Jump.Velocity = State.Jump.SineAngle = State.Jump.CosineAngle = State.Jump.XYSpeed = 0.0f;

			IsMovementUpdated = true;

			return WriteStateAsync();
		}
	}
}
