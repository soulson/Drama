using Drama.Core.Interfaces;
using Drama.Core.Interfaces.Numerics;
using Drama.Shard.Grains.Units;
using Drama.Shard.Interfaces.Characters;
using Drama.Shard.Interfaces.Objects;
using Drama.Shard.Interfaces.Units;
using Orleans;
using Orleans.Providers;
using System.Threading.Tasks;

namespace Drama.Shard.Grains.Characters
{
	[StorageProvider(ProviderName = StorageProviders.DynamicWorld)]
	public sealed class Character : AbstractCharacter<CharacterEntity>, ICharacter
	{

	}

	public abstract class AbstractCharacter<TEntity> : AbstractUnit<TEntity>, ICharacter<TEntity>
		where TEntity : CharacterEntity, new()
	{
		public async Task<TEntity> Create(string name, string account, string shard, Race race, Class @class, Sex sex, byte skin, byte face, byte hairStyle, byte hairColor, byte facialHair)
		{
			if (IsExists)
				throw new CharacterAlreadyExistsException($"{GetType().Name} with objectid {State.Id} already exists");

			State.Account = account;
			State.Class = @class;
			State.Enabled = true;
			State.Face = face;
			State.FacialHair = facialHair;
			State.HairColor = hairColor;
			State.HairStyle = hairStyle;
			State.Id = new ObjectID(this.GetPrimaryKeyLong());
			State.Level = 1;
			State.MapId = 0; // TODO
			State.Name = name;
			State.Orientation = 0.0f; // TODO
			State.Position = new Vector3(); // TODO
			State.Race = race;
			State.Sex = sex;
			State.Shard = shard;
			State.Skin = skin;
			State.ZoneId = 0; // TODO

			await WriteStateAsync();

			return State;
		}
	}
}
