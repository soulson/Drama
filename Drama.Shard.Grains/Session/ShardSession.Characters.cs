using Drama.Auth.Interfaces;
using Drama.Auth.Interfaces.Account;
using Drama.Core.Interfaces.Security;
using Drama.Core.Interfaces.Utilities;
using Drama.Shard.Interfaces.Characters;
using Drama.Shard.Interfaces.Protocol;
using Drama.Shard.Interfaces.Session;
using Orleans;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Drama.Shard.Grains.Session
{
	public partial class ShardSession
	{
		public async Task<IList<CharacterEntity>> GetCharacterList()
		{
			if (AuthenticatedIdentity == null)
				throw new SessionStateException($"{nameof(GetCharacterList)} can only be called while authenticated");

			var result = new List<CharacterEntity>();
			var characterList = GrainFactory.GetGrain<ICharacterList>(ShardName);
			var characterIds = await characterList.GetCharactersByAccount(AuthenticatedIdentity);

			foreach(var id in characterIds)
			{
				var character = GrainFactory.GetGrain<ICharacter>(id);
				result.Add(await character.GetEntity());
			}

			return result;
		}
	}
}
