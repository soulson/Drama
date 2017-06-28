using Drama.Shard.Interfaces.Objects;
using System;

namespace Drama.Shard.Interfaces.Units
{
	public class UnitException : ObjectException
	{
		public UnitException(string message) : base(message) { }
		public UnitException(string message, Exception cause) : base(message, cause) { }
	}
}
