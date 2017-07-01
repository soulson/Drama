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

namespace Drama.Auth.Interfaces.Protocol
{
	public sealed class LogonProofResponse : IOutPacket
	{
		public AuthResponse Result { get; set; }
		public BigInteger M2 { get; set; }

		public void Write(Stream stream)
		{
			stream.WriteByte((byte)AuthRequestOpcode.LogonProof);
			stream.WriteByte((byte)Result);

			if(Result == AuthResponse.Success)
			{
				stream.Write(M2.ToByteArray(20), 0, 20);
				stream.Write(new byte[4], 0, 4);
			}
		}
	}
}
