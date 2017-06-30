using Drama.Core.Interfaces.Networking;
using Drama.Core.Interfaces.Numerics;
using System.IO;
using System.Text;

namespace Drama.Shard.Interfaces.Protocol
{
	public sealed class LoginVerifyWorldRequest : IOutPacket
	{
		public ShardServerOpcode Opcode { get; } = ShardServerOpcode.LoginVerifyWorld;

		public int MapId { get; set; }
		public Vector3 Position { get; set; }
		public float Orientation { get; set; }

		public void Write(Stream stream)
		{
			using (var writer = new BinaryWriter(stream, Encoding.UTF8, true))
			{
				writer.Write((ushort)Opcode);

				writer.Write(MapId);
				writer.Write(Position.X);
				writer.Write(Position.Y);
				writer.Write(Position.Z);
				writer.Write(Orientation);
			}
		}
	}
}
