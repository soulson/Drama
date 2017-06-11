using System;

namespace Drama.Core.Interfaces
{
	public class DramaException : Exception
	{
		public DramaException(string message) : base(message) { }
		public DramaException(string message, Exception cause) : base(message, cause) { }
	}
}
