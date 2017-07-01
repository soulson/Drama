using Drama.Core.Interfaces;
using Drama.Core.Interfaces.Networking;
using System;
using System.Collections.Generic;
using System.IO;

namespace Drama.Core.Gateway.Networking
{
	public abstract class PacketReader
  {
    private const int InitialCapacity = 2048;

    private readonly MemoryStream buffer;

    public PacketReader()
    {
      buffer = new MemoryStream(InitialCapacity);
    }
		
		/// <remarks>
		/// The reason this method yields is in case packets start showing up partially. They could be streamed
		/// into the buffer and read when complete, which may result in 0, 1, or many packets being returned
		/// by this method at a time. However, this is hard to do, and has not yet seemed necessary.
		/// </remarks>
		public IEnumerable<IInPacket> ProcessData(ArraySegment<byte> data)
    {
			ProcessOnce(data);

			IInPacket packet;

			lock (buffer)
			{
				buffer.Position = 0;
				buffer.Write(data.Array, 0, data.Count);

				buffer.Position = 0;
				packet = CreatePacket(buffer);

				if (packet is UnimplementedPacket)
					yield break;

				if (packet == null)
					throw new DramaException("packet returned by CreatePacket was null! this may mean that the stream is corrupt or that we're receiving non-whole packets");

				buffer.Position = ReadOffset;
				if (!packet.Read(buffer))
					throw new DramaException("packet.Read returned false! this may mean that the stream is corrupt or that we're receiving non-whole packets");
			}

			yield return packet;
    }

    /// <remarks>
    /// Returns a new IPacket object of the proper type to read the next packet on the stream or null if
    /// there is not enough data to determine the type of the appropriate IPacket. If there is no
    /// appropriate IPacket type to handle the packet, then an UnimplementedPacket object with the correct
    /// size is returned.
    /// </remarks>
    protected abstract IInPacket CreatePacket(Stream stream);

		protected abstract int ReadOffset { get; }

		protected virtual void ProcessOnce(ArraySegment<byte> input) { }
  }
}
