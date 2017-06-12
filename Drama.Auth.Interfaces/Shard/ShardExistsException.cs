using System;

namespace Drama.Auth.Interfaces.Shard
{
	public class ShardExistsException : ShardException
	{
		public ShardExistsException(string message) : base(message) { }
		public ShardExistsException(string message, Exception cause) : base(message, cause) { }
	}
}
