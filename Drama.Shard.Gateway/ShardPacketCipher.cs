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
