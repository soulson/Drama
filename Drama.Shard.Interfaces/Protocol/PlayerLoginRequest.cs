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

using Drama.Core.Interfaces.Networking;
using Drama.Shard.Interfaces.Objects;
using System.IO;
using System.Text;

namespace Drama.Shard.Interfaces.Protocol
{
	[ClientPacket(ShardClientOpcode.PlayerLogin)]
	public sealed class PlayerLoginRequest : IInPacket
	{
		public ObjectID CharacterId { get; set; }

		public bool Read(Stream stream)
		{
			try
			{
				using (var reader = new BinaryReader(stream, Encoding.UTF8, true))
					CharacterId = new ObjectID(reader.ReadInt64());

				return true;
			}
			catch (EndOfStreamException)
			{
				return false;
			}
		}
	}
}
