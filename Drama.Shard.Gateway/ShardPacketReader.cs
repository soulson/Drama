using Drama.Shard.Interfaces.Protocol;
using Drama.Core.Gateway.Networking;
using Drama.Core.Interfaces.Networking;
using System;
using System.IO;

namespace Drama.Shard.Gateway
{
	public class ShardPacketReader : PacketReader
  {
		private const int ClientPacketHeaderSize = 6;

		private readonly ShardPacketCipher packetCipher;

		public ShardPacketReader(ShardPacketCipher packetCipher)
		{
			this.packetCipher = packetCipher;
		}

    protected override IInPacket CreatePacket(Stream stream)
    {
			if (stream.Length - stream.Position < ClientPacketHeaderSize)
				return null;

			var initialPosition = stream.Position;
			var header = new byte[ClientPacketHeaderSize];
			stream.Read(header, 0, header.Length);

			packetCipher.DecryptHeader(new ArraySegment<byte>(header));

			// the size is encoded in big endian and doesn't include the size of the size itself
			var size = sizeof(ushort) + BitConverter.ToUInt16(new[] { header[1], header[0] }, 0);
			var ordinal = BitConverter.ToUInt32(header, sizeof(ushort));

			if (stream.Length - initialPosition < size)
				return null;

			Console.WriteLine($"received packet from client: type = {(ShardClientOpcode)ordinal}, total size = {size}");

			return new UnimplementedPacket(size);
    }
  }
}
