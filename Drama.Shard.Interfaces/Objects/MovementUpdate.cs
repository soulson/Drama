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

using Drama.Core.Interfaces.Numerics;
using Drama.Shard.Interfaces.Units;
using Drama.Shard.Interfaces.WorldObjects;
using Orleans.Concurrency;
using System;

namespace Drama.Shard.Interfaces.Objects
{
	/// <summary>
	/// MovementUpdate objects are a type of ObjectUpdate block that represents
	/// position, velocity, and delta information for persistent object motion.
	/// </summary>
	[Immutable]
	public class MovementUpdate
	{
		// fields from ObjectEntity
		public Vector3 Position { get; }
		public float Orientation { get; }

		// fields from UnitEntity
		public int MoveTime { get; }
		public int FallTime { get; }
		public float MovePitch { get; }
		public MovementFlags MoveFlags { get; }
		public float SpeedWalking { get; }
		public float SpeedRunning { get; }
		public float SpeedRunningBack { get; }
		public float SpeedSwimming { get; }
		public float SpeedSwimmingBack { get; }
		public float SpeedTurning { get; }
		public float JumpVelocity { get; }
		public float JumpSineAngle { get; }
		public float JumpCosineAngle { get; }
		public float JumpXYSpeed { get; }
		public float UnknownSplineThingy { get; }

		/// <summary>
		/// Creates a MovementUpdate for a PersistentObject.
		/// </summary>
		/// <param name="entity">Cannot be null</param>
		public MovementUpdate(WorldObjectEntity entity)
		{
			if (entity == null)
				throw new ArgumentNullException(nameof(entity));
			
			Position = entity.Position;
			Orientation = entity.Orientation;
		}

		/// <summary>
		/// Creates a MovementUpdate for a Unit.
		/// </summary>
		/// <param name="entity">Cannot be null</param>
		public MovementUpdate(UnitEntity entity) : this(entity as WorldObjectEntity)
		{
			MoveTime = entity.MoveTime;
			FallTime = entity.FallTime;
			MovePitch = entity.MovePitch;
			MoveFlags = entity.MoveFlags;
			SpeedWalking = entity.MoveSpeed.Walking;
			SpeedRunning = entity.MoveSpeed.Running;
			SpeedRunningBack = entity.MoveSpeed.RunningBack;
			SpeedSwimming = entity.MoveSpeed.Swimming;
			SpeedSwimmingBack = entity.MoveSpeed.SwimmingBack;
			SpeedTurning = entity.MoveSpeed.Turning;
			JumpVelocity = entity.Jump.Velocity;
			JumpSineAngle = entity.Jump.SineAngle;
			JumpCosineAngle = entity.Jump.CosineAngle;
			JumpXYSpeed = entity.Jump.XYSpeed;
			UnknownSplineThingy = entity.UnknownSplineThingy;
		}
	}
}
