using System;
using System.IO;

namespace Drama.Core.Interfaces.Networking
{
  public interface IOutPacket
  {
    void Write(Stream stream);
  }
}
