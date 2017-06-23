using System;

namespace Drama.Shard.Interfaces.Session
{
	public class AuthenticationFailedException : SessionException
	{
		public AuthenticationFailedException(string message) : base(message) { }
		public AuthenticationFailedException(string message, Exception cause) : base(message, cause) { }
	}
}
