using System;

namespace Drama.Auth.Interfaces.Account
{
	public class AccountDoesNotExistException : AccountException
	{
		public AccountDoesNotExistException(string message) : base(message) { }
		public AccountDoesNotExistException(string message, Exception cause) : base(message, cause) { }
	}
}
