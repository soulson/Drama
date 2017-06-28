using Drama.Core.Interfaces;
using System;

namespace Drama.Shard.Interfaces.Objects
{
	public class ObjectException : DramaException
	{
		public ObjectException(string message) : base(message) { }
		public ObjectException(string message, Exception cause) : base(message, cause) { }
	}
}
