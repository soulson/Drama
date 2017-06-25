using Drama.Shard.Interfaces.Units;
using System;

namespace Drama.Shard.Interfaces.Characters
{
	public class CharacterEntity : UnitEntity
	{
		public string Name { get; set; }
		public string Account { get; set; }

		public byte Skin { get; set; }
		public byte Face { get; set; }
		public byte HairStyle { get; set; }
		public byte HairColor { get; set; }
		public byte FacialHair { get; set; }

		public int ZoneId { get; set; }
	}
}
