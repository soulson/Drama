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
	public sealed class MovementOutPacket : AbstractOutPacket
	{
		private readonly ShardServerOpcode opcode;

		public ObjectID ObjectId { get; set; }
		public MovementFlags MovementFlags { get; set; }
		public int Time { get; set; }
		public Vector3 Position { get; set; }
		public float Orientation { get; set; }
		public TransportInfo Transport { get; set; }
		public float? Pitch { get; set; }
		public int FallTime { get; set; }
		public FallingInfo Falling { get; set; }

		public MovementOutPacket(ShardServerOpcode opcode)
		{
			this.opcode = opcode;
		}

		public override ShardServerOpcode Opcode => opcode;

		protected override void Write(BinaryWriter writer)
		{
			writer.Write(ObjectId.GetPacked());
			writer.Write((int)MovementFlags);
			writer.Write(Time);
			writer.Write(Position.X);
			writer.Write(Position.Y);
			writer.Write(Position.Z);
			writer.Write(Orientation);

			if(MovementFlags.HasFlag(MovementFlags.OnTransport))
			{
				writer.Write(Transport.Id);
				writer.Write(Transport.Position.X);
				writer.Write(Transport.Position.Y);
				writer.Write(Transport.Position.Z);
				writer.Write(Transport.Orientation);
			}

			if (MovementFlags.HasFlag(MovementFlags.ModeSwimming))
				writer.Write(Pitch.Value);

			writer.Write(FallTime);

			if(MovementFlags.HasFlag(MovementFlags.ModeFalling))
			{
				writer.Write(Falling.Velocity);
				writer.Write(Falling.SinAngle);
				writer.Write(Falling.CosAngle);
				writer.Write(Falling.XYSpeed);
			}
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
