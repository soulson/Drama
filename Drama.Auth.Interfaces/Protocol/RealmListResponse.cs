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

using Drama.Auth.Interfaces.Shard;
using Drama.Core.Interfaces.Networking;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Drama.Auth.Interfaces.Protocol
{
	public sealed class RealmListResponse : IOutPacket
	{
		public IList<ShardEntity> ShardList { get; } = new List<ShardEntity>();

		public void Write(Stream stream)
		{
			using (var writer = new BinaryWriter(stream, Encoding.UTF8, true))
			{
				writer.Write((byte)AuthRequestOpcode.RealmList);

				var sizePosition = stream.Position;
				writer.Write((ushort)7); // number of remaining bytes

				var sizeStartPosition = stream.Position;
				writer.Write(0);
				writer.Write((byte)Math.Min(ShardList.Count, byte.MaxValue)); // number of shards in list

				for (int i = 0; i < Math.Min(ShardList.Count, byte.MaxValue); ++i)
				{
					var shard = ShardList[i];

					writer.Write((int)shard.ShardType);
					writer.Write((byte)shard.ShardFlags);
					writer.Write(Encoding.UTF8.GetBytes(shard.Name));
					writer.Write((byte)0); // null terminator
					writer.Write(Encoding.UTF8.GetBytes($"{shard.Address}:{shard.Port}"));
					writer.Write((byte)0); // null terminator
					writer.Write(0.0f); // "population"
					writer.Write((byte)0); // TODO: character count
					writer.Write((byte)0); // "category";
					writer.Write((byte)0); // ?
				}

				writer.Write((short)2); // ?

				var finalPosition = stream.Position;

				stream.Position = sizePosition;
				writer.Write((ushort)(finalPosition - sizeStartPosition));
				stream.Position = finalPosition;
			}
		}
	}
}
