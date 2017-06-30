using Drama.Core.Interfaces.Networking;
using System.IO;
using System.Text;

namespace Drama.Shard.Interfaces.Protocol
{
	public sealed class AccountDataTimesRequest : IOutPacket
	{
		public ShardServerOpcode Opcode { get; } = ShardServerOpcode.AccountDataTimes;

		public byte[] Data { get; } = new byte[128];

		public void Write(Stream stream)
		{
			using (var writer = new BinaryWriter(stream, Encoding.UTF8, true))
			{
				writer.Write((ushort)Opcode);

				writer.Write(Data);
			}
		}
	}
}
