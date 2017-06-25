using Drama.Core.Interfaces.Networking;
using System;
using System.Text;
using System.IO;
using Drama.Shard.Interfaces.Units;
using Drama.Core.Interfaces.Utilities;

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
