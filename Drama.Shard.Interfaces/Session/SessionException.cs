using Drama.Core.Interfaces;
using System;

namespace Drama.Shard.Interfaces.Session
{
	public class SessionException : DramaException
	{
		public SessionException(string message) : base(message) { }
		public SessionException(string message, Exception cause) : base(message, cause) { }
	}
}
