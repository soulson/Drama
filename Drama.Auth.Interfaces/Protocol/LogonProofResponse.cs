using Drama.Auth.Interfaces.Protocol;
using Drama.Core.Interfaces.Networking;
using Drama.Core.Interfaces.Utilities;
using System;
using System.IO;
using System.Numerics;

namespace Drama.Auth.Interfaces.Protocol
{
	public sealed class LogonProofResponse : IOutPacket
	{
		public AuthResponseOpcode Result { get; set; }
		public BigInteger M2 { get; set; }

		public void Write(Stream stream)
		{
			stream.WriteByte((byte)AuthRequestOpcode.LogonProof);
			stream.WriteByte((byte)Result);

			if(Result == AuthResponseOpcode.Success)
			{
				stream.Write(M2.ToByteArray(20), 0, 20);
				stream.Write(new byte[4], 0, 4);
			}
		}
	}
}
