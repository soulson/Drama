using Drama.Shard.Interfaces.Units;
using Orleans;
using System;
using System.Threading.Tasks;

namespace Drama.Shard.Interfaces.Characters
{
	public interface ICharacter : ICharacter<CharacterEntity>, IGrainWithIntegerKey
	{

	}

	public interface ICharacter<TEntity> : IUnit<TEntity>, IGrainWithIntegerKey
		where TEntity : CharacterEntity, new()
	{
		Task<TEntity> Create(string name, string account, string shard, Race race, Class @class, Sex sex, byte skin, byte face, byte hairStyle, byte hairColor, byte facialHair);
	}
}
