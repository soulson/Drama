using Drama.Shard.Interfaces.Characters;
using Drama.Shard.Interfaces.Protocol;
using Drama.Shard.Interfaces.Session;
using System.Collections.Generic;
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

		public async Task CreateCharacter(CharacterCreateRequest request)
		{
			var characterList = GrainFactory.GetGrain<ICharacterList>(ShardName);
			var id = await characterList.AddCharacter(request.Name, AuthenticatedIdentity);
			var character = GrainFactory.GetGrain<ICharacter>(id);
			var entity = await character.Create(request.Name, AuthenticatedIdentity, ShardName, request.Race, request.Class, request.Sex, request.Skin, request.Face, request.HairStyle, request.HairColor, request.FacialHair);
		}
	}
}
