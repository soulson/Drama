using Drama.Shard.Interfaces.Objects;
using Orleans;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Drama.Shard.Interfaces.Characters
{
	// key is the shard name
	public interface ICharacterList : IGrainWithStringKey
	{
		Task<IList<ObjectID>> GetCharactersByAccount(string accountName);
		Task<ObjectID?> GetCharacterByName(string characterName);

		Task<ObjectID> AddCharacter(string characterName, string accountName);
	}
}
