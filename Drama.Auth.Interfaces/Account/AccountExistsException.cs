using System;

namespace Drama.Auth.Interfaces.Account
{
	public class AccountExistsException : AccountException
	{
		public AccountExistsException(string message) : base(message) { }
		public AccountExistsException(string message, Exception cause) : base(message, cause) { }
	}
}
