using System;
using System.IO;

namespace Drama.Core.Interfaces.Networking
{
  public interface IPacket
  {
    /// <remarks>
    /// Reads this packet from the current stream. If there is not enough data on the stream to read the
    /// entire packet, then this method returns false.
    /// </remarks>
    /// <param name="stream">stream from which to read</param>
    /// <returns>true if entire packet was read; false if not</returns>
    bool Read(Stream stream);

    void Write(Stream stream);
  }
}
