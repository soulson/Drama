using Drama.Core.Interfaces.Networking;
using System;
using System.Text;
using System.IO;

namespace Drama.Shard.Interfaces.Protocol
{
	public sealed class CharacterCreateResponse : IOutPacket
	{
		public ShardServerOpcode Opcode { get; } = ShardServerOpcode.CharacterCreate;

		public CharacterCreateResponseCode Response { get; set; }

		public void Write(Stream stream)
		{
			using (var writer = new BinaryWriter(stream, Encoding.UTF8, true))
			{
				writer.Write((ushort)Opcode);
				writer.Write((byte)Response);
			}
		}
	}
}
