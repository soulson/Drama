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

using Drama.Core.Interfaces.Utilities;
using Drama.Shard.Interfaces.Chat;
using System.IO;
using System.Text;

namespace Drama.Shard.Interfaces.Protocol
{
	[ClientPacket(ShardClientOpcode.ChatMessage)]
	public sealed class ChatMessageRequest : AbstractInPacket
	{
		public ChatMessageType MessageType { get; set; }
		public ChatLanguage Language { get; set; }
		public string Message { get; set; }
		public string TargetName { get; set; }

		protected override void Read(BinaryReader reader)
		{
			MessageType = (ChatMessageType)reader.ReadInt32();
			Language = (ChatLanguage)reader.ReadInt32();

			if (MessageType == ChatMessageType.Channel || MessageType == ChatMessageType.Whisper)
				TargetName = reader.ReadNullTerminatedString(Encoding.UTF8);

			Message = reader.ReadNullTerminatedString(Encoding.UTF8);
		}
	}
}
