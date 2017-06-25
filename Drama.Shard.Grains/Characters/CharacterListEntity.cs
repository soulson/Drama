using Drama.Shard.Interfaces.Objects;
using System;
using System.Collections.Generic;

namespace Drama.Shard.Grains.Characters
{
	public class CharacterListEntity
	{
		public int NextId { get; set; } = 0;
		public SortedDictionary<string, ObjectID> CharacterByName { get; } = new SortedDictionary<string, ObjectID>();
		public SortedDictionary<string, IList<ObjectID>> CharactersByAccount { get; } = new SortedDictionary<string, IList<ObjectID>>();
	}
}
