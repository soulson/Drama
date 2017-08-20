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
using Drama.Shard.Interfaces.Objects;
using System.IO;
using System.Text;

namespace Drama.Shard.Interfaces.Protocol
{
	public sealed class ChatMessageResponse : AbstractOutPacket
	{
		public override ShardServerOpcode Opcode => ShardServerOpcode.ChatMessage;

		public ChatMessageType MessageType { get; set; }
		public ChatLanguage Language { get; set; }
		public string SenderName { get; set; }
		public ObjectID TargetId { get; set; }
		public ObjectID SenderId { get; set; }
		public string ChannelName { get; set; }
		public int SenderRank { get; set; }
		public string Message { get; set; }
		public ChatTag Tag { get; set; }

		protected override void Write(BinaryWriter writer)
		{
			writer.Write((sbyte)MessageType);
			writer.Write((int)Language);

			switch (MessageType)
			{
				case ChatMessageType.Say:
				case ChatMessageType.Yell:
				case ChatMessageType.Party:
					writer.Write(SenderId);
					writer.Write(SenderId);
					break;

				case ChatMessageType.Channel:
					writer.WriteNullTerminatedString(ChannelName, Encoding.UTF8);
					writer.Write(SenderRank);
					writer.Write(SenderId);
					break;

				case ChatMessageType.MonsterSay:
				case ChatMessageType.MonsterYell:
					writer.Write(SenderId);
					writer.Write(Encoding.UTF8.GetByteCount(SenderName) + 1);
					writer.WriteNullTerminatedString(SenderName, Encoding.UTF8);
					writer.Write(TargetId);
					break;

				case ChatMessageType.MonsterEmote:
				case ChatMessageType.MonsterWhisper:
					writer.Write(Encoding.UTF8.GetByteCount(SenderName) + 1);
					writer.WriteNullTerminatedString(SenderName, Encoding.UTF8);
					writer.Write(TargetId);
					break;

				default:
					writer.Write(SenderId);
					break;
			}

			writer.Write(Encoding.UTF8.GetByteCount(Message) + 1);
			writer.WriteNullTerminatedString(Message, Encoding.UTF8);
			writer.Write((byte)Tag);
		}
	}
}
