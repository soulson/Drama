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
using Drama.Core.Interfaces.Utilities;
using Drama.Shard.Interfaces.Units;
using System.IO;
using System.Text;

namespace Drama.Shard.Interfaces.Protocol
{
	[ClientPacket(ShardClientOpcode.CharacterCreate)]
	public sealed class CharacterCreateRequest : IInPacket
	{
		public string Name { get; set; }
		public Race Race { get; set; }
		public Class Class { get; set; }
		public Sex Sex { get; set; }
		public byte Skin { get; set; }
		public byte Face { get; set; }
		public byte HairStyle { get; set; }
		public byte HairColor { get; set; }
		public byte FacialHair { get; set; }
		public byte OutfitId { get; set; }

		public bool Read(Stream stream)
		{
			try
			{
				using (var reader = new BinaryReader(stream, Encoding.UTF8, true))
				{
					Name = reader.ReadNullTerminatedString(Encoding.UTF8);

					Race = (Race)reader.ReadByte();
					Class = (Class)reader.ReadByte();
					Sex = (Sex)reader.ReadByte();
					Skin = reader.ReadByte();
					Face = reader.ReadByte();
					HairStyle = reader.ReadByte();
					HairColor = reader.ReadByte();
					FacialHair = reader.ReadByte();
					OutfitId = reader.ReadByte();
				}

				return true;
			}
			catch (EndOfStreamException)
			{
				return false;
			}
		}
	}
}
