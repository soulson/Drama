using System;
using System.Numerics;

namespace Drama.Auth.Interfaces.Account
{
	public class AccountEntity
	{
		public bool Enabled { get; set; }
		public string Name { get; set; }
		public BigInteger Verifier { get; set; }
		public BigInteger Salt { get; set; }
		public AccountSecurityLevel SecurityLevel { get; set; }
	}
}
