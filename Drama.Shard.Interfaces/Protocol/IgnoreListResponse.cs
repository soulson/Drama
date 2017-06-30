using System.IO;

namespace Drama.Shard.Interfaces.Protocol
{
	public sealed class IgnoreListResponse : AbstractOutPacket
	{
		public override ShardServerOpcode Opcode => ShardServerOpcode.IgnoreList;

		protected override void Write(BinaryWriter writer)
		{
			// TODO: IgnoreListResponse
			writer.Write((byte)0);
		}
	}
}
