using Drama.Shard.Interfaces.Objects;
using System.Collections.Generic;

namespace Drama.Shard.Grains.Characters
{
	public class CharacterListEntity
	{
		// important to start this at 1. a character with id 0 will crash the client on load
		public long NextId { get; set; } = 1L;
		public SortedDictionary<string, ObjectID> CharacterByName { get; } = new SortedDictionary<string, ObjectID>();
		public SortedDictionary<string, IList<ObjectID>> CharactersByAccount { get; } = new SortedDictionary<string, IList<ObjectID>>();
	}
}
