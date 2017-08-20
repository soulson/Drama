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

				default:
					// not yet implemented or nonsense
					return Task.CompletedTask;
			}
		}
	}
}
