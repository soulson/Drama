using System.IO;

namespace Drama.Shard.Interfaces.Protocol
{
	public sealed class LoginInitializeSpellsRequest : AbstractOutPacket
	{
		public override ShardServerOpcode Opcode => ShardServerOpcode.LoginInitializeSpells;

		protected override void Write(BinaryWriter writer)
		{
			// TODO: LoginInitializeSpells
			writer.Write((byte)0); // unknown
			writer.Write((ushort)0); // known spell count
			writer.Write((ushort)0); // spell cooldown count
		}
	}
}
