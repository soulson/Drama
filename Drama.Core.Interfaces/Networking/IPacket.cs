using System;
using System.IO;

namespace Drama.Core.Interfaces.Networking
{
  public interface IPacket
  {
    void Read(Stream stream);
    void Write(Stream stream);
  }
}
