/* 
 * The Drama project: what you get when a bunch of actors try to host a game.
 * Copyright (C) 2017 Soulson
 *
 * This program is free software: you can redistribute it and/or modify
 * it under the terms of the GNU Affero General Public License as
 * published by the Free Software Foundation, either version 3 of the
 * License, or (at your option) any later version.
 *
 * This program is distributed in the hope that it will be useful,
 * but WITHOUT ANY WARRANTY; without even the implied warranty of
 * MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
 * GNU Affero General Public License for more details.
 *
 * You should have received a copy of the GNU Affero General Public License
 * along with this program.  If not, see <http://www.gnu.org/licenses/>.
 */

using Drama.Auth.Interfaces;
using Drama.Core.Interfaces;
using Drama.Shard.Interfaces.Characters;
using Drama.Shard.Interfaces.Objects;
using Orleans;
using Orleans.Providers;
using System.Collections.Generic;
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

			var idGenerator = GrainFactory.GetGrain<IObjectIDGenerator>(0);
			var newCharacterId = await idGenerator.GenerateObjectId(ObjectID.Type.Player);

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
			if (State.CharacterByName.ContainsKey(characterName))
				return Task.FromResult<ObjectID?>(State.CharacterByName[characterName]);
			else
				return Task.FromResult<ObjectID?>(null);
		}

		public Task RemoveCharacter(string characterName, string accountName)
		{
			if (State.CharacterByName.ContainsKey(characterName))
			{
				var characterId = State.CharacterByName[characterName];

				if (State.CharactersByAccount.ContainsKey(accountName))
				{
					if (!State.CharactersByAccount[accountName].Remove(characterId))
						GetLogger().Warn($"tried to remove {characterId} ({characterName}) from character/account relationship list but it did not exist");
				}
				else
					GetLogger().Warn($"tried to remove {characterId} ({characterName}) from character/account relationship list but account {accountName} did not exist");
			}

			if(!State.CharacterByName.Remove(characterName))
				GetLogger().Warn($"tried to remove {characterName} from character list but it did not exist");

			return WriteStateAsync();
		}
	}
}
