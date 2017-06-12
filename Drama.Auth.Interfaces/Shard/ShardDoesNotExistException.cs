using System;

namespace Drama.Auth.Interfaces.Shard
{
	public class ShardDoesNotExistException : ShardException
	{
		public ShardDoesNotExistException(string message) : base(message) { }
		public ShardDoesNotExistException(string message, Exception cause) : base(message, cause) { }
	}
}
