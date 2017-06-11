using Drama.Core.Interfaces;
using System;

namespace Drama.Auth.Interfaces.Account
{
	public class AccountStateException : DramaException
	{
		public AccountStateException(string message) : base(message) { }
		public AccountStateException(string message, Exception cause) : base(message, cause) { }
	}
}
