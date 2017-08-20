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

		[Handler(typeof(PlayerLogoutRequest))]
		private Task HandlePlayerLogoutRequest(PlayerLogoutRequest request)
		{
			return ShardSession.Logout();
		}
	}
}
