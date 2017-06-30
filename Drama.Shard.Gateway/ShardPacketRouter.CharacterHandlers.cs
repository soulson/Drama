using Drama.Shard.Interfaces.Characters;
using Drama.Shard.Interfaces.Protocol;
using System;
using System.Threading.Tasks;

namespace Drama.Shard.Gateway
{
	public partial class ShardPacketRouter
	{
		[Handler(typeof(CharacterListRequest))]
		private async Task HandleCharacterList(CharacterListRequest request)
		{
			var response = new CharacterListResponse();
			var characterList = await ShardSession.GetCharacterList();

			// TODO: this equipment list obviously needs revisited once items are implemented
			var equipmentCount = Enum.GetValues(typeof(EquipmentSlot)).Length;
			var equipmentDisplayIds = new int[equipmentCount];
			var equipmentInventoryTypes = new byte[equipmentCount];

			foreach (var character in characterList)
			{
				response.AddCharacter(character.Id, character.Name, character.Race, character.Class, character.Sex, character.Skin, character.Face, character.HairStyle,
					character.HairColor, character.FacialHair, character.Level, character.ZoneId, character.MapId, character.Position, 0, 0, true, 0, 0, 0, 0, 0, equipmentDisplayIds, equipmentInventoryTypes);
			}

			ForwardPacket(response);
		}

		[Handler(typeof(CharacterCreateRequest))]
		private async Task HandleCharacterCreate(CharacterCreateRequest request)
		{
			try
			{
				await ShardSession.CreateCharacter(request);
				ForwardPacket(new CharacterCreateResponse() { Response = CharacterCreateResponseCode.Success });
			}
			catch (CharacterAlreadyExistsException)
			{
				ForwardPacket(new CharacterCreateResponse() { Response = CharacterCreateResponseCode.NameTaken });
			}
		}

		[Handler(typeof(PlayerLoginRequest))]
		private Task HandlePlayerLogin(PlayerLoginRequest request)
		{
			return ShardSession.Login(request.CharacterId);
		}
	}
}
