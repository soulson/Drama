using Drama.Core.Interfaces.Networking;
using System;
using System.IO;

namespace Drama.Auth.Interfaces.Protocol
{
  public sealed class RealmListRequest : IInPacket
  {
    public bool Read(Stream stream)
    {
			if (stream.Length - stream.Position < 1)
				return false;

			// skip the opcode
			stream.ReadByte();

			return true;
    }
  }
}
