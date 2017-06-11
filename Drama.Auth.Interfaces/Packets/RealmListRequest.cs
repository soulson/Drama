using Drama.Core.Interfaces.Networking;
using System;
using System.IO;

namespace Drama.Auth.Interfaces.Packets
{
  public sealed class RealmListRequest : IInPacket
  {
    public bool Read(Stream stream)
    {
      throw new NotImplementedException();
    }
  }
}
