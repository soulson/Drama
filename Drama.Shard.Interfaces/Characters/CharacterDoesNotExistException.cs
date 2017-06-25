using System;

namespace Drama.Shard.Interfaces.Characters
{
	public class CharacterDoesNotExistException : CharacterException
	{
		public CharacterDoesNotExistException(string message) : base(message) { }
		public CharacterDoesNotExistException(string message, Exception cause) : base(message, cause) { }
	}
}
