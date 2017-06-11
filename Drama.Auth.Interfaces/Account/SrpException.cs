using System;

namespace Drama.Auth.Interfaces.Account
{
	public class SrpException : AccountException
	{
		public SrpException(string message) : base(message) { }
		public SrpException(string message, Exception cause) : base(message, cause) { }
	}
}
