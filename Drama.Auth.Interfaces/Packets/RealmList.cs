using Drama.Core.Interfaces.Networking;
using System;
using System.IO;

namespace Drama.Auth.Interfaces.Packets
{
  public sealed class RealmList : IPacket
  {
    public bool Read(Stream stream)
    {
      throw new NotImplementedException();
    }

    public void Write(Stream stream)
    {
      throw new NotImplementedException();
    }
  }
}
