using Drama.Auth.Interfaces.Protocol;
using Drama.Core.Gateway.Networking;
using Drama.Core.Interfaces;
using Drama.Core.Interfaces.Networking;
using System;
using System.IO;

namespace Drama.Auth.Gateway
{
	public class AuthPacketReader : PacketReader
	{
		protected override int ReadOffset => 1;

		protected override IInPacket CreatePacket(Stream stream, out int packetSize)
    {
      var ordinal = stream.ReadByte();

			if (ordinal < 0)
			{
				packetSize = 0;
				return null;
			}

      var opcode = (AuthRequestOpcode)ordinal;

      switch (opcode)
      {
        case AuthRequestOpcode.LogonChallenge:
					{
						// 23 + [byte @ 22]
						stream.Seek(22, SeekOrigin.Current);
						var identitySize = stream.ReadByte();

						if (identitySize < 0)
							throw new DramaException($"{nameof(identitySize)} could not be read");

						packetSize = 23 + identitySize;

						return new LogonChallengeRequest();
					}
        case AuthRequestOpcode.LogonProof:
					packetSize = 75;
          return new LogonProofRequest();
        case AuthRequestOpcode.RealmList:
					packetSize = 5;
          return new RealmListRequest();
        case AuthRequestOpcode.ReconnectChallenge:
					{
						// 34 + [byte @ 33]
						stream.Seek(32, SeekOrigin.Current);
						var identitySize = stream.ReadByte();

						if (identitySize < 0)
						{
							packetSize = 0;
							return null;
						}
						else
						{
							packetSize = 34 + identitySize;
							return new UnimplementedPacket(33 + identitySize);
						}
					}
        case AuthRequestOpcode.ReconnectProof:
					packetSize = 58;
          return new UnimplementedPacket(57);
        default:
          throw new DramaException($"unrecognized auth opcode value {ordinal}");
      }
    }
  }
}
