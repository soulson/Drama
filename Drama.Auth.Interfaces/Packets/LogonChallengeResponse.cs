﻿using Drama.Auth.Interfaces.Protocol;
using Drama.Core.Interfaces.Networking;
using System;
using System.IO;
using System.Numerics;
using System.Text;

namespace Drama.Auth.Interfaces.Packets
{
	public sealed class LogonChallengeResponse : IOutPacket
	{
		private const byte GLength = 1;
		private const byte NLength = 32;
		private const byte SecurityFlags = 0;

		public AuthResponseOpcode Result { get; set; }
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

				if(Result == AuthResponseOpcode.Success)
				{
					// obvious todo
				}
			}
		}
	}
}
