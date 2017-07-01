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

using Drama.Core.Interfaces.Networking;
using Drama.Core.Interfaces.Utilities;
using System.IO;
using System.Numerics;
using System.Text;

namespace Drama.Auth.Interfaces.Protocol
{
	public sealed class LogonChallengeResponse : IOutPacket
	{
		private const byte GLength = 1;
		private const byte NLength = 32;
		private const byte SecurityFlags = 0;

		public AuthResponse Result { get; set; }
		public BigInteger B { get; set; }
		public BigInteger N { get; set; }
		public byte G { get; set; }
		public BigInteger Salt { get; set; }
		public BigInteger RandomNumber { get; set; }

		public void Write(Stream stream)
		{
			using (var writer = new BinaryWriter(stream, Encoding.UTF8, true))
			{
				writer.Write((byte)AuthRequestOpcode.LogonChallenge);
				writer.Write((byte)0);
				writer.Write((byte)Result);

				if(Result == AuthResponse.Success)
				{
					writer.Write(B.ToByteArray(32));
					writer.Write(GLength);
					writer.Write(G);
					writer.Write(NLength);
					writer.Write(N.ToByteArray(NLength));
					writer.Write(Salt.ToByteArray(32));
					writer.Write(RandomNumber.ToByteArray(16));
					writer.Write(SecurityFlags);
				}
			}
		}
	}
}
