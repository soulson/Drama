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
using Drama.Shard.Interfaces.Objects;
using Drama.Shard.Interfaces.Units;
using System.IO;

namespace Drama.Shard.Interfaces.Protocol
{
	[ClientPacket(ShardClientOpcode.MoveFallLand)]
	[ClientPacket(ShardClientOpcode.MoveHeartbeat)]
	[ClientPacket(ShardClientOpcode.MoveJump)]
	[ClientPacket(ShardClientOpcode.MovePitchStartDown)]
	[ClientPacket(ShardClientOpcode.MovePitchStartUp)]
	[ClientPacket(ShardClientOpcode.MovePitchStop)]
	[ClientPacket(ShardClientOpcode.MoveSetOrientation)]
	[ClientPacket(ShardClientOpcode.MoveSetPitch)]
	[ClientPacket(ShardClientOpcode.MoveStartBackward)]
	[ClientPacket(ShardClientOpcode.MoveStartForward)]
	[ClientPacket(ShardClientOpcode.MoveStop)]
	[ClientPacket(ShardClientOpcode.MoveStrafeStartLeft)]
	[ClientPacket(ShardClientOpcode.MoveStrafeStartRight)]
	[ClientPacket(ShardClientOpcode.MoveStrafeStop)]
	[ClientPacket(ShardClientOpcode.MoveSwimStart)]
	[ClientPacket(ShardClientOpcode.MoveSwimStop)]
	[ClientPacket(ShardClientOpcode.MoveTurnStartLeft)]
	[ClientPacket(ShardClientOpcode.MoveTurnStartRight)]
	[ClientPacket(ShardClientOpcode.MoveTurnStop)]
	public class MovementInPacket : AbstractInPacket
	{
		public MovementFlags MovementFlags { get; set; }
		public int Time { get; set; }
		public Vector3 Position { get; set; }
		public float Orientation { get; set; }
		public TransportInfo Transport { get; set; }
		public float? Pitch { get; set; }
		public int FallTime { get; set; }
		public FallingInfo Falling { get; set; }

		protected override void Read(BinaryReader reader)
		{
			MovementFlags = (MovementFlags)reader.ReadInt32();
			Time = reader.ReadInt32();
			Position = new Vector3(reader.ReadSingle(), reader.ReadSingle(), reader.ReadSingle());
			Orientation = reader.ReadSingle();

			if (MovementFlags.HasFlag(MovementFlags.OnTransport))
			{
				Transport = new TransportInfo()
				{
					Id = new ObjectID(reader.ReadInt64()),
					Position = new Vector3(reader.ReadSingle(), reader.ReadSingle(), reader.ReadSingle()),
					Orientation = reader.ReadSingle(),
				};
			}
			else
				Transport = null;

			if (MovementFlags.HasFlag(MovementFlags.ModeSwimming))
				Pitch = reader.ReadSingle();
			else
				Pitch = null;

			FallTime = reader.ReadInt32();

			if (MovementFlags.HasFlag(MovementFlags.ModeFalling))
			{
				Falling = new FallingInfo()
				{
					Velocity = reader.ReadSingle(),
					SinAngle = reader.ReadSingle(),
					CosAngle = reader.ReadSingle(),
					XYSpeed = reader.ReadSingle(),
				};
			}
			else
				Falling = null;
		}

		public class TransportInfo
		{
			public ObjectID Id { get; set; }
			public Vector3 Position { get; set; }
			public float Orientation { get; set; }
		}

		public class FallingInfo
		{
			public float Velocity { get; set; }
			public float SinAngle { get; set; }
			public float CosAngle { get; set; }
			public float XYSpeed { get; set; }
		}
	}
}
