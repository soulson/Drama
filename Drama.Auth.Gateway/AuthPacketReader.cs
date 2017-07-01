/* 
 * The Drama project: what you get when a bunch of actors try to host a game.
 * Copyright (C) 2017 Soulson
 *
 * This program is free software: you can redistribute it and/or modify
 * it under the terms of the GNU Affero General Public License as
 * published by the Free Software Foundation, either version 3 of the
 * License, or (at your option) any later version.
 *
 * This program is distributed in the hope that it will be useful,
 * but WITHOUT ANY WARRANTY; without even the implied warranty of
 * MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
 * GNU Affero General Public License for more details.
 *
 * You should have received a copy of the GNU Affero General Public License
 * along with this program.  If not, see <http://www.gnu.org/licenses/>.
 */

using Drama.Auth.Interfaces.Protocol;
using Drama.Core.Gateway.Networking;
using Drama.Core.Interfaces;
using Drama.Core.Interfaces.Networking;
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
