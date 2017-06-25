using Drama.Core.Interfaces;
using Drama.Shard.Interfaces.Characters;
using Drama.Shard.Interfaces.Objects;
using Orleans;
using Orleans.Providers;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Drama.Shard.Grains.Characters
{
	[StorageProvider(ProviderName = StorageProviders.DynamicWorld)]
	public class CharacterList : Grain<CharacterListEntity>, ICharacterList
	{
		public async Task<ObjectID> AddCharacter(string characterName, string accountName)
		{
			if (State.CharacterByName.ContainsKey(characterName))
				throw new CharacterAlreadyExistsException($"character with the name {characterName} already exists on shard {this.GetPrimaryKeyString()}");
			
			if (!State.CharactersByAccount.ContainsKey(accountName))
				State.CharactersByAccount.Add(accountName, new List<ObjectID>());

			var newCharacterId = new ObjectID(State.NextId++, ObjectID.Type.Player);
			State.CharactersByAccount[accountName].Add(newCharacterId);
			State.CharacterByName[characterName] = newCharacterId;

			await WriteStateAsync();

			return newCharacterId;
		}

		public Task<IList<ObjectID>> GetCharactersByAccount(string accountName)
		{
			if (State.CharactersByAccount.ContainsKey(accountName))
				return Task.FromResult(State.CharactersByAccount[accountName]);
			else
				return Task.FromResult<IList<ObjectID>>(new List<ObjectID>());
		}

		public Task<ObjectID?> GetCharacterByName(string characterName)
		{
			if (State.CharactersByAccount.ContainsKey(characterName))
				return Task.FromResult<ObjectID?>(State.CharacterByName[characterName]);
			else
				return Task.FromResult<ObjectID?>(null);
		}
	}
}
