using System;

namespace Drama.Shard.Interfaces.Session
{
	public class SessionStateException : SessionException
	{
		public SessionStateException(string message) : base(message) { }
		public SessionStateException(string message, Exception cause) : base(message, cause) { }
	}
}
