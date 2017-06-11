using System;
using System.Numerics;

namespace Drama.Auth.Interfaces.Account
{
	public sealed class SrpInitialParameters
	{
		public BigInteger B { get; }
		public byte G { get; }
		public BigInteger N { get; }
		public BigInteger Salt { get; }
		public BigInteger RandomNumber { get; }

		public SrpInitialParameters(BigInteger b, byte g, BigInteger n, BigInteger salt, BigInteger randomNumber)
		{
			B = b;
			G = g;
			N = n;
			Salt = salt;
			RandomNumber = randomNumber;
		}
	}
}
