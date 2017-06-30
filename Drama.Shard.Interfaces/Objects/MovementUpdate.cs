using Drama.Core.Interfaces.Numerics;
using Drama.Shard.Interfaces.Units;
using Orleans.Concurrency;
using System;

namespace Drama.Shard.Interfaces.Objects
{
	[Immutable]
	public class MovementUpdate
	{
		public ObjectUpdateFlags UpdateFlags { get; }

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

		public MovementUpdate(ObjectEntity entity, ObjectUpdateFlags updateFlags)
		{
			if (entity == null)
				throw new ArgumentNullException(nameof(entity));

			UpdateFlags = updateFlags;
			Position = entity.Position;
			Orientation = entity.Orientation;
		}

		public MovementUpdate(UnitEntity entity, ObjectUpdateFlags updateFlags) : this(entity as ObjectEntity, updateFlags)
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
