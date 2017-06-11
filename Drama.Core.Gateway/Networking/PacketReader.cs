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

    public IEnumerable<IInPacket> ProcessData(ArraySegment<byte> data)
    {
      var initialPosition = buffer.Position;
      buffer.Write(data.Array, data.Offset, data.Count);
      buffer.Position = initialPosition;
      
      while (true)
      {
        initialPosition = buffer.Position;
        var packet = CreatePacket(buffer);
        buffer.Position = initialPosition;

        void OnIncompletePacket()
        {
          var remainder = new byte[buffer.Length - buffer.Position];
          buffer.Read(remainder, 0, remainder.Length);

          buffer.Position = 0;
          buffer.Write(remainder, 0, remainder.Length);
          buffer.SetLength(buffer.Position);
        }

        if (packet == null)
        {
          OnIncompletePacket();
          yield break;
        }
        else if (packet is UnimplementedPacket)
        {
          // TODO: log
          if(!packet.Read(buffer))
          {
            OnIncompletePacket();
            yield break;
          }
        }
        else
        {
          if (packet.Read(buffer))
            yield return packet;
          else
          {
            OnIncompletePacket();
            yield break;
          }
        }
      }
    }

    /// <remarks>
    /// Returns a new IPacket object of the proper type to read the next packet on the stream or null if
    /// there is not enough data to determine the type of the appropriate IPacket. If there is no
    /// appropriate IPacket type to handle the packet, then an UnimplementedPacket object with the correct
    /// size is returned.
    /// </remarks>
    protected abstract IInPacket CreatePacket(Stream stream);
  }
}
