using Drama.Shard.Interfaces.Units;
using Orleans;
using System;
using System.Threading.Tasks;

namespace Drama.Shard.Interfaces.Characters
{
	public interface ICharacter : IGrainWithIntegerKey
	{
		Task<bool> Exists();
		Task<CharacterEntity> GetEntity();

		Task<CharacterEntity> Create(string name, string account, string shard, Race race, Class @class, Sex sex, byte skin, byte face, byte hairStyle, byte hairColor, byte facialHair);
	}
}
