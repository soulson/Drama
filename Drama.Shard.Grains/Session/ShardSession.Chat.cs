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
using Drama.Shard.Interfaces.Chat;
using Drama.Shard.Interfaces.Protocol;
using System.Threading.Tasks;

namespace Drama.Shard.Grains.Session
{
	public partial class ShardSession
	{
		public Task SendChatMessage(ChatMessageRequest request)
		{
			VerifyIngame();
			
			switch (request.MessageType)
			{
				case ChatMessageType.Say:
					return ActiveCharacter.Say(request.Message, request.Language);

				case ChatMessageType.Yell:
					return ActiveCharacter.Yell(request.Message, request.Language);

				case ChatMessageType.Whisper:
					return SendWhisper(request);

				default:
					// not yet implemented or nonsense
					return Task.CompletedTask;
			}
		}

		private async Task SendWhisper(ChatMessageRequest request)
		{
			var characterList = GrainFactory.GetGrain<ICharacterList>(ShardName);
			var characterId = await characterList.GetCharacterByName(request.TargetName);

			if(characterId != null)
			{
				var character = GrainFactory.GetGrain<ICharacter>(characterId.Value);

				// technically a race condition, but probably okay
				if(await character.IsOnline())
				{
					var myEntity = await ActiveCharacter.GetCharacterEntity();
					await character.ReceiveWhisper(myEntity.Id, request.Message, request.Language);

					var chatConfirm = new ChatMessageResponse()
					{
						// TODO: language support needed here
						Language = ChatLanguage.Universal,
						Message = request.Message,
						MessageType = ChatMessageType.WhisperConfirm,
						// sender and target are reversed for WhisperConfirm
						SenderId = characterId.Value,
						Tag = ChatTag.None,
						TargetId = myEntity.Id,
					};

					await Send(chatConfirm);
				}
				else
				{
					// TODO: "character is not online" notification
				}
			}
			else
			{
				// TODO: "character does not exist" notification
			}
		}
	}
}
