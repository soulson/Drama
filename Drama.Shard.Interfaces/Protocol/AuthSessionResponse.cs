using Drama.Core.Interfaces.Networking;
using System.IO;

namespace Drama.Shard.Interfaces.Protocol
{
	public sealed class AuthSessionResponse : IOutPacket
	{
		public ShardServerOpcode Opcode { get; } = ShardServerOpcode.AuthResponse;

		public AuthResponse Response { get; set; }

		public void Write(Stream stream)
		{
			stream.WriteByte((byte)Response);

			if (Response == AuthResponse.Success)
				stream.Write(new byte[9], 0, 9);
		}
	}
}
