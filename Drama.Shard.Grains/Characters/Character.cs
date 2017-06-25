using Drama.Core.Interfaces;
using Drama.Core.Interfaces.Numerics;
using Drama.Shard.Interfaces.Characters;
using Drama.Shard.Interfaces.Units;
using Orleans;
using Orleans.Providers;
using System.Threading.Tasks;

namespace Drama.Shard.Grains.Characters
{
	[StorageProvider(ProviderName = StorageProviders.DynamicWorld)]
	public class Character : Grain<CharacterEntity>, ICharacter
	{
		private bool IsExists => State.Enabled;

		public async Task<CharacterEntity> Create(string name, string account, string shard, Race race, Class @class, Sex sex, byte skin, byte face, byte hairStyle, byte hairColor, byte facialHair)
		{
			if (IsExists)
				throw new CharacterAlreadyExistsException($"character with objectid {State.Id} already exists");

			var characterList = GrainFactory.GetGrain<ICharacterList>(shard);

			// this will also throw CharacterAlreadyExistsException if there is a name collision
			var objectId = await characterList.AddCharacter(name, account);

			State.Class = @class;
			State.Enabled = true;
			State.Face = face;
			State.FacialHair = facialHair;
			State.HairColor = hairColor;
			State.HairStyle = hairStyle;
			State.Id = objectId;
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

		public Task<bool> Exists()
			=> Task.FromResult(IsExists);

		public Task<CharacterEntity> GetEntity()
		{
			if (!IsExists)
				throw new CharacterDoesNotExistException($"character {this.GetPrimaryKeyString()} does not exist");

			return Task.FromResult(State);
		}
	}
}
