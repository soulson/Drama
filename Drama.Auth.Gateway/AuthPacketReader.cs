using Drama.Auth.Interfaces.Packets;
using Drama.Core.Gateway.Networking;
using Drama.Core.Interfaces.Networking;
using System;
using System.IO;

namespace Drama.Auth.Gateway
{
  public class AuthPacketReader : PacketReader
  {
    protected override IPacket CreatePacket(Stream stream)
    {
      var ordinal = stream.ReadByte();

      if (ordinal < 0)
        return null;

      var opcode = (AuthRequestOpcode)ordinal;

      switch (opcode)
      {
        case AuthRequestOpcode.LogonChallenge:
          return new LogonChallenge();
        case AuthRequestOpcode.LogonProof:
          return new LogonProof();
        case AuthRequestOpcode.RealmList:
          return new RealmList();
        case AuthRequestOpcode.ReconnectChallenge:
          // 34 + [byte @ 33]
          stream.Seek(32, SeekOrigin.Current);
          var identitySize = stream.ReadByte();

          if (identitySize < 0)
            return null;
          else
            return new UnimplementedPacket(34 + identitySize);
        case AuthRequestOpcode.ReconnectProof:
          return new UnimplementedPacket(58);
        default:
          throw new Exception($"unrecognized auth opcode value {ordinal}");
      }
    }
  }
}
