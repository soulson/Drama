using Drama.Core.Interfaces.Networking;
using System.IO;
using System.Text;

namespace Drama.Shard.Interfaces.Protocol
{
	public abstract class AbstractOutPacket : IOutPacket
	{
		public abstract ShardServerOpcode Opcode { get; }

		public void Write(Stream stream)
		{
			using (var writer = new BinaryWriter(stream, Encoding.UTF8, true))
			{
				writer.Write((ushort)Opcode);
				Write(writer);
			}
		}

		protected abstract void Write(BinaryWriter writer);
	}
}
