using Drama.Core.Interfaces;
using System;

namespace Drama.Shard.Interfaces.Characters
{
	public class CharacterException : DramaException
	{
		public CharacterException(string message) : base(message) { }
		public CharacterException(string message, Exception cause) : base(message, cause) { }
	}
}
