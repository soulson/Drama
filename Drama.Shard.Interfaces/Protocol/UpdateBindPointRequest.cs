using Drama.Core.Interfaces.Numerics;
using System.IO;

namespace Drama.Shard.Interfaces.Protocol
{
	public sealed class UpdateBindPointRequest : AbstractOutPacket
	{
		public override ShardServerOpcode Opcode => ShardServerOpcode.BindPointUpdate;

		public Vector3 Position { get; set; }
		public int MapId { get; set; }
		public int ZoneId { get; set; }

		protected override void Write(BinaryWriter writer)
		{
			writer.Write(Position.X);
			writer.Write(Position.Y);
			writer.Write(Position.Z);
			writer.Write(MapId);
			writer.Write(ZoneId);
		}
	}
}
