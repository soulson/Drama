using Drama.Core.Interfaces.Networking;
using System;
using System.IO;
using System.Text;

namespace Drama.Shard.Interfaces.Protocol
{
	public sealed class AuthSessionResponse : IOutPacket
	{
		public ShardServerOpcode Opcode { get; } = ShardServerOpcode.AuthResponse;

		public AuthResponseCode Response { get; set; }

		public void Write(Stream stream)
		{
			using (var writer = new BinaryWriter(stream, Encoding.UTF8, true))
			{
				writer.Write((ushort)Opcode);
				writer.Write((byte)Response);

				if (Response == AuthResponseCode.Success)
				{
					writer.Write(0);
					writer.Write((byte)0);
					writer.Write(0);
				}
			}
		}
	}
}
