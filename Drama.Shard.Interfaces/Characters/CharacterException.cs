using Drama.Shard.Interfaces.Units;
using System;

namespace Drama.Shard.Interfaces.Characters
{
	public class CharacterException : UnitException
	{
		public CharacterException(string message) : base(message) { }
		public CharacterException(string message, Exception cause) : base(message, cause) { }
	}
}
