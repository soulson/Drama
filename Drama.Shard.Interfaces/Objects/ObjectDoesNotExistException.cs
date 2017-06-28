using System;

namespace Drama.Shard.Interfaces.Objects
{
	public class ObjectDoesNotExistException : ObjectException
	{
		public ObjectDoesNotExistException(string message) : base(message) { }
		public ObjectDoesNotExistException(string message, Exception cause) : base(message, cause) { }
	}
}
