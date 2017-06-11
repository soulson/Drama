using Drama.Core.Interfaces;
using System;

namespace Drama.Auth.Interfaces.Account
{
	public class AccountException : DramaException
	{
		public AccountException(string message) : base(message) { }
		public AccountException(string message, Exception cause) : base(message, cause) { }
	}
}
