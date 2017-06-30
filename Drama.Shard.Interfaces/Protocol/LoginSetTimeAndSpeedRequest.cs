using Drama.Core.Interfaces.Utilities;
using System;
using System.IO;

namespace Drama.Shard.Interfaces.Protocol
{
	public sealed class LoginSetTimeAndSpeedRequest : AbstractOutPacket
	{
		public override ShardServerOpcode Opcode => ShardServerOpcode.LoginSetTimeAndSpeed;

		public float GameSpeed { get; set; }
		public DateTime ServerTime { get; set; }

		protected override void Write(BinaryWriter writer)
		{
			writer.Write(DateTimes.GetBitfield(ServerTime));
			writer.Write(GameSpeed);
		}
	}
}
