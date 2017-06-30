using Drama.Core.Interfaces.Networking;
using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using Drama.Shard.Interfaces.Objects;
using Drama.Shard.Interfaces.Units;

namespace Drama.Shard.Interfaces.Protocol
{
	public sealed class ObjectUpdateRequest : IOutPacket
	{
		public ShardServerOpcode Opcode { get; } = ShardServerOpcode.ObjectUpdate;

		public bool IsSelf { get; set; }
		public ObjectUpdate ObjectUpdate { get; set; }

		public void Write(Stream stream)
		{
			if (ObjectUpdate == null)
				throw new ArgumentNullException(nameof(ObjectUpdate));

			using (var writer = new BinaryWriter(stream, Encoding.UTF8, true))
			{
				// "create" updates look different than "update" updates
				if(ObjectUpdate.UpdateType == ObjectUpdateType.CreateObject || ObjectUpdate.UpdateType == ObjectUpdateType.CreateObject2)
				{
					// neither of these can be null if this is a create update
					if (ObjectUpdate.ValuesUpdate == null)
						throw new ArgumentNullException(nameof(ObjectUpdate.ValuesUpdate));
					if (ObjectUpdate.MovementUpdate == null)
						throw new ArgumentNullException(nameof(ObjectUpdate.MovementUpdate));

					writer.Write((byte)ObjectUpdate.UpdateType);
					writer.Write(ObjectUpdate.ObjectId.GetPacked());
					writer.Write((byte)ObjectUpdate.TypeId);

					WriteMovementBlock(writer);
					WriteValuesBlock(writer);
				}
				else
				{
					// when not in a creation block, movement and values updates are independent
					if(ObjectUpdate.MovementUpdate != null)
					{
						writer.Write((byte)ObjectUpdateType.Movement);
						writer.Write(ObjectUpdate.ObjectId);
						WriteMovementBlock(writer);
					}

					if(ObjectUpdate.ValuesUpdate != null)
					{
						writer.Write((byte)ObjectUpdateType.Values);
						writer.Write(ObjectUpdate.ObjectId.GetPacked());
						WriteValuesBlock(writer);
					}
				}
			}
		}

		private void WriteMovementBlock(BinaryWriter writer)
		{
			if(ObjectUpdate.MovementUpdate != null)
			{
				var updateFlags = ObjectUpdate.UpdateFlags;
				if (IsSelf)
					updateFlags |= ObjectUpdateFlags.Self;

				writer.Write((byte)updateFlags);

				if(updateFlags.HasFlag(ObjectUpdateFlags.Living))
				{
					var moveFlags = ObjectUpdate.MovementUpdate.MoveFlags;

					writer.Write((int)moveFlags);
					writer.Write(ObjectUpdate.MovementUpdate.MoveTime);
					writer.Write(ObjectUpdate.MovementUpdate.Position.X);
					writer.Write(ObjectUpdate.MovementUpdate.Position.Y);
					writer.Write(ObjectUpdate.MovementUpdate.Position.Z);
					writer.Write(ObjectUpdate.MovementUpdate.Orientation);

					// TODO: support transports

					if (moveFlags.HasFlag(MovementFlags.ModeSwimming))
						writer.Write(ObjectUpdate.MovementUpdate.MovePitch);

					writer.Write(ObjectUpdate.MovementUpdate.FallTime);

					if(moveFlags.HasFlag(MovementFlags.ModeFalling))
					{
						writer.Write(ObjectUpdate.MovementUpdate.JumpVelocity);
						writer.Write(ObjectUpdate.MovementUpdate.JumpSineAngle);
						writer.Write(ObjectUpdate.MovementUpdate.JumpCosineAngle);
						writer.Write(ObjectUpdate.MovementUpdate.JumpXYSpeed);
					}

					if (moveFlags.HasFlag(MovementFlags.SplineElevation))
						writer.Write(ObjectUpdate.MovementUpdate.UnknownSplineThingy);

					writer.Write(ObjectUpdate.MovementUpdate.SpeedWalking);
					writer.Write(ObjectUpdate.MovementUpdate.SpeedRunning);
					writer.Write(ObjectUpdate.MovementUpdate.SpeedRunningBack);
					writer.Write(ObjectUpdate.MovementUpdate.SpeedSwimming);
					writer.Write(ObjectUpdate.MovementUpdate.SpeedSwimmingBack);
					writer.Write(ObjectUpdate.MovementUpdate.SpeedTurning);

					// TODO: support spline motion
				}
				else if(updateFlags.HasFlag(ObjectUpdateFlags.HasPosition))
				{
					writer.Write(ObjectUpdate.MovementUpdate.Position.X);
					writer.Write(ObjectUpdate.MovementUpdate.Position.Y);
					writer.Write(ObjectUpdate.MovementUpdate.Position.Z);
					writer.Write(ObjectUpdate.MovementUpdate.Orientation);
				}

				if (updateFlags.HasFlag(ObjectUpdateFlags.MaskId))
					writer.Write(ObjectUpdate.ObjectId.MaskId);

				if (updateFlags.HasFlag(ObjectUpdateFlags.All))
					writer.Write(1);

				// TODO: support fullguid, transports
			}
		}

		private void WriteValuesBlock(BinaryWriter writer)
		{
			if(ObjectUpdate.ValuesUpdate != null)
			{
				writer.Write(ObjectUpdate.ValuesUpdate.BlockCount);

				// inefficient?
				foreach (byte b in ObjectUpdate.ValuesUpdate.UpdateMask)
					writer.Write(b);
				foreach (int i in ObjectUpdate.ValuesUpdate.Fields)
					writer.Write(i);
			}
		}
	}
}
