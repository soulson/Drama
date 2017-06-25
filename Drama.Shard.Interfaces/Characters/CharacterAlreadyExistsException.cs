using System;

namespace Drama.Shard.Interfaces.Characters
{
	public class CharacterAlreadyExistsException : CharacterException
	{
		public CharacterAlreadyExistsException(string message) : base(message) { }
		public CharacterAlreadyExistsException(string message, Exception cause) : base(message, cause) { }
	}
}
