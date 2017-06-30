using Drama.Core.Interfaces.Networking;
using System.IO;
using System.Text;

namespace Drama.Shard.Interfaces.Protocol
{
	public sealed class LoginSetRestStartRequest : IOutPacket
	{
		public ShardServerOpcode Opcode { get; } = ShardServerOpcode.LoginSetRestStart;

		public int Value { get; set; } = 0;

		public void Write(Stream stream)
		{
			using (var writer = new BinaryWriter(stream, Encoding.UTF8, true))
			{
				writer.Write((ushort)Opcode);

				writer.Write(Value);
			}
		}
	}
}
