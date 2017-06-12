using Drama.Core.Interfaces;
using System;

namespace Drama.Auth.Interfaces.Shard
{
	public class ShardException : DramaException
	{
		public ShardException(string message) : base(message) { }
		public ShardException(string message, Exception cause) : base(message, cause) { }
	}
}
