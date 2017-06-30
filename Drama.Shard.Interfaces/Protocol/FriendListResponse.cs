using System.IO;

namespace Drama.Shard.Interfaces.Protocol
{
	public sealed class FriendListResponse : AbstractOutPacket
	{
		public override ShardServerOpcode Opcode => ShardServerOpcode.FriendList;

		protected override void Write(BinaryWriter writer)
		{
			// TODO: FriendListResponse
			writer.Write((byte)0);
		}
	}
}
