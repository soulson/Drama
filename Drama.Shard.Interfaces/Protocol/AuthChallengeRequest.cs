using Drama.Core.Interfaces.Networking;
using Drama.Core.Interfaces.Utilities;
using System.IO;
using System.Numerics;
using System.Text;

namespace Drama.Shard.Interfaces.Protocol
{
	public sealed class AuthChallengeRequest : IOutPacket
	{
		public ShardServerOpcode Opcode { get; } = ShardServerOpcode.AuthChallenge;

		public int Seed { get; set; }
		public BigInteger RandomNumber { get; set; }

		public void Write(Stream stream)
		{
			using (var writer = new BinaryWriter(stream, Encoding.UTF8, true))
			{
				writer.Write((ushort)Opcode);
				writer.Write(Seed);
				writer.Write(RandomNumber.ToByteArray(32));
			}
		}
	}
}
