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

using Drama.Core.Interfaces.Utilities;
using System;
using System.Numerics;

namespace Drama.Shard.Gateway
{
	public sealed class ShardPacketCipher
	{
		public const int KeySize = 40;
		public const int SendLength = 4;
		public const int ReceiveLength = 6;

		private bool initialized;
		private byte[] sessionKey;

		private byte sendI, sendJ;
		private byte recvI, recvJ;

		public ShardPacketCipher()
		{
			initialized = false;
		}

		public void Initialize(BigInteger sessionKey)
		{
			this.sessionKey = sessionKey.ToByteArray(KeySize);
			sendI = sendJ = 0;
			recvI = recvJ = 0;
			initialized = true;
		}

		public void EncryptHeader(ArraySegment<byte> packet)
		{
			if (!initialized)
				return;

			if (packet.Count < SendLength)
				throw new ArgumentException($"packet to encrypt must be at least {SendLength} bytes in length");

			for (int t = 0; t < SendLength; ++t)
			{
				sendI %= KeySize;
				byte x = unchecked((byte)((packet.Array[t + packet.Offset] ^ sessionKey[sendI]) + sendJ));
				++sendI;
				packet.Array[t + packet.Offset] = sendJ = x;
			}
		}

		public void DecryptHeader(ArraySegment<byte> packet)
		{
			if (!initialized)
				return;

			if (packet.Count < ReceiveLength)
				throw new ArgumentException($"packet to decrypt must be at least {ReceiveLength} bytes in length");

			for (int t = 0; t < ReceiveLength; ++t)
			{
				recvI %= KeySize;
				byte x = unchecked((byte)((packet.Array[t + packet.Offset] - recvJ) ^ sessionKey[recvI]));
				++recvI;
				recvJ = packet.Array[t + packet.Offset];
				packet.Array[t + packet.Offset] = x;
			}
		}
	}
}
