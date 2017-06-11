using System;
using System.Numerics;

namespace Drama.Auth.Interfaces.Account
{
	public sealed class SrpResult
	{
		public bool Match { get; }
		public BigInteger M2 { get; }

		public SrpResult(bool match, BigInteger m2)
		{
			Match = match;
			M2 = m2;
		}
	}
}
