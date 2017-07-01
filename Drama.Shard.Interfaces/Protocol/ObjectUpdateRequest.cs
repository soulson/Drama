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

using Drama.Core.Interfaces.Networking;
using Drama.Shard.Interfaces.Objects;
using Drama.Shard.Interfaces.Units;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Drama.Shard.Interfaces.Protocol
{
	public sealed class ObjectUpdateRequest : IOutPacket
	{
		public ShardServerOpcode Opcode { get; } = ShardServerOpcode.ObjectUpdate;

		/// <summary>
		/// The ObjectID of the character that the session this request is being sent to is logged in as.
		/// This is used to apply the Self update flag when necessary.
		/// </summary>
		public ObjectID TargetObjectId { get; set; }
		public IList<ObjectUpdate> ObjectUpdates { get; } = new List<ObjectUpdate>();
		public bool HasTransport { get; set; }

		public void Write(Stream stream)
		{
			using (var writer = new BinaryWriter(stream, Encoding.UTF8, true))
			{
				writer.Write((ushort)Opcode);

				writer.Write(ObjectUpdates.Count);
				writer.Write(HasTransport ? (byte)1 : (byte)0);

				foreach (var update in ObjectUpdates)
				{
					// "create" updates look different than "update" updates
					if (update.UpdateType == ObjectUpdateType.CreateObject || update.UpdateType == ObjectUpdateType.CreateObject2)
					{
						// neither of these can be null if this is a create update
						if (update.ValuesUpdate == null)
							throw new ArgumentNullException(nameof(ObjectUpdate.ValuesUpdate));
						if (update.MovementUpdate == null)
							throw new ArgumentNullException(nameof(ObjectUpdate.MovementUpdate));

						writer.Write((byte)update.UpdateType);
						writer.Write(update.ObjectId.GetPacked());
						writer.Write((byte)update.TypeId);

						WriteMovementBlock(writer, update);
						WriteValuesBlock(writer, update);
					}
					else
					{
						// when not in a creation block, movement and values updates are independent
						if (update.MovementUpdate != null)
						{
							writer.Write((byte)ObjectUpdateType.Movement);
							writer.Write(update.ObjectId);
							WriteMovementBlock(writer, update);
						}

						if (update.ValuesUpdate != null)
						{
							writer.Write((byte)ObjectUpdateType.Values);
							writer.Write(update.ObjectId.GetPacked());
							WriteValuesBlock(writer, update);
						}
					}
				}
			}
		}

		private void WriteMovementBlock(BinaryWriter writer, ObjectUpdate update)
		{
			if(update.MovementUpdate != null)
			{
				var updateFlags = update.UpdateFlags;
				if (update.ObjectId == TargetObjectId)
					updateFlags |= ObjectUpdateFlags.Self;

				writer.Write((byte)updateFlags);

				if(updateFlags.HasFlag(ObjectUpdateFlags.Living))
				{
					var moveFlags = update.MovementUpdate.MoveFlags;

					writer.Write((int)moveFlags);
					writer.Write(update.MovementUpdate.MoveTime);
					writer.Write(update.MovementUpdate.Position.X);
					writer.Write(update.MovementUpdate.Position.Y);
					writer.Write(update.MovementUpdate.Position.Z);
					writer.Write(update.MovementUpdate.Orientation);

					// TODO: support transports

					if (moveFlags.HasFlag(MovementFlags.ModeSwimming))
						writer.Write(update.MovementUpdate.MovePitch);

					writer.Write(update.MovementUpdate.FallTime);

					if(moveFlags.HasFlag(MovementFlags.ModeFalling))
					{
						writer.Write(update.MovementUpdate.JumpVelocity);
						writer.Write(update.MovementUpdate.JumpSineAngle);
						writer.Write(update.MovementUpdate.JumpCosineAngle);
						writer.Write(update.MovementUpdate.JumpXYSpeed);
					}

					if (moveFlags.HasFlag(MovementFlags.SplineElevation))
						writer.Write(update.MovementUpdate.UnknownSplineThingy);

					writer.Write(update.MovementUpdate.SpeedWalking);
					writer.Write(update.MovementUpdate.SpeedRunning);
					writer.Write(update.MovementUpdate.SpeedRunningBack);
					writer.Write(update.MovementUpdate.SpeedSwimming);
					writer.Write(update.MovementUpdate.SpeedSwimmingBack);
					writer.Write(update.MovementUpdate.SpeedTurning);

					// TODO: support spline motion
				}
				else if(updateFlags.HasFlag(ObjectUpdateFlags.HasPosition))
				{
					writer.Write(update.MovementUpdate.Position.X);
					writer.Write(update.MovementUpdate.Position.Y);
					writer.Write(update.MovementUpdate.Position.Z);
					writer.Write(update.MovementUpdate.Orientation);
				}

				if (updateFlags.HasFlag(ObjectUpdateFlags.MaskId))
					writer.Write(update.ObjectId.MaskId);

				if (updateFlags.HasFlag(ObjectUpdateFlags.All))
					writer.Write(1);

				// TODO: support fullguid, transports
			}
		}

		private void WriteValuesBlock(BinaryWriter writer, ObjectUpdate update)
		{
			if(update.ValuesUpdate != null)
			{
				writer.Write(update.ValuesUpdate.BlockCount);

				// inefficient?
				foreach (byte b in update.ValuesUpdate.UpdateMask)
					writer.Write(b);
				foreach (int i in update.ValuesUpdate.Fields)
					writer.Write(i);
			}
		}
	}
}
