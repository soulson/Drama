using System.IO;

namespace Drama.Shard.Interfaces.Protocol
{
	public sealed class InitializeWorldStateRequest : AbstractOutPacket
	{
		public override ShardServerOpcode Opcode => ShardServerOpcode.InitializeWorldState;

		public int MapId { get; set; }
		public int ZoneId { get; set; }

		protected override void Write(BinaryWriter writer)
		{
			// TODO: InitializeWorldStateRequest
			writer.Write(MapId);
			writer.Write(ZoneId);
			writer.Write((short)0); // special map data for outdoor pvp areas and such
		}
	}
}
