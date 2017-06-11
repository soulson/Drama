using Drama.Core.Interfaces.Networking;
using Drama.Core.Interfaces.Utilities;
using System;
using System.IO;
using System.Numerics;

namespace Drama.Auth.Interfaces.Protocol
{
  public sealed class LogonProofRequest : IInPacket
  {
		public BigInteger A { get; set; }
		public BigInteger M1 { get; set; }

    public bool Read(Stream stream)
    {
			if (stream.Length - stream.Position < 75)
				return false;

			// skip the opcode
			stream.Seek(1, SeekOrigin.Current);

			var aBytes = new byte[32];
			var m1Bytes = new byte[20];

			stream.Read(aBytes, 0, aBytes.Length);
			stream.Read(m1Bytes, 0, m1Bytes.Length);

			// skip remaining stuff
			stream.Seek(22, SeekOrigin.Current);

			A = BigIntegers.FromUnsignedByteArray(aBytes);
			M1 = BigIntegers.FromUnsignedByteArray(m1Bytes);

			return true;
    }
  }
}
