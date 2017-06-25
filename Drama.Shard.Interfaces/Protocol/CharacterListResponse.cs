using Drama.Core.Interfaces.Networking;
using Drama.Core.Interfaces.Numerics;
using Drama.Core.Interfaces.Utilities;
using Drama.Shard.Interfaces.Objects;
using Drama.Shard.Interfaces.Units;
using Orleans.Concurrency;
using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.IO;
using System.Text;

namespace Drama.Shard.Interfaces.Protocol
{
	public class CharacterListResponse : IOutPacket
	{
		public ShardServerOpcode Opcode { get; } = ShardServerOpcode.CharacterList;

		private IList<CharacterListEntry> CharacterList { get; } = new List<CharacterListEntry>();

		public void AddCharacter(ObjectID id, string name, Race race, Class @class, Sex sex, byte skin, byte face, byte hairStyle, byte hairColor, byte facialHair, byte level, int zoneId, int mapId, Vector3 position, int guildId, int playerFlags, bool isFirstLogin, int petDisplayId, int petLevel, int petFamily, int firstBagDisplayId, byte firstBagInventoryType, int[] equipmentDisplayIds, byte[] equipmentInventoryTypes)
		{
			if (equipmentDisplayIds.Length != equipmentInventoryTypes.Length)
				throw new ArgumentOutOfRangeException(nameof(equipmentInventoryTypes), $"length of {nameof(equipmentInventoryTypes)} must be equal to length of {nameof(equipmentDisplayIds)}");

			var slotBuilder = ImmutableList.CreateBuilder<EquipmentSlotEntry>();
			for (int i = 0; i < equipmentDisplayIds.Length; ++i)
				slotBuilder.Add(new EquipmentSlotEntry(equipmentDisplayIds[i], equipmentInventoryTypes[i]));

			CharacterList.Add(new CharacterListEntry(id, name, race, @class, sex, skin, face, hairStyle, hairColor, facialHair, level, zoneId, mapId, position, guildId, playerFlags, isFirstLogin, petDisplayId, petLevel, petFamily, slotBuilder.ToImmutable(), firstBagDisplayId, firstBagInventoryType));
		}

		public void Write(Stream stream)
		{
			using (var writer = new BinaryWriter(stream, Encoding.UTF8, true))
			{
				writer.Write((ushort)Opcode);
				writer.Write(checked((byte)CharacterList.Count));

				foreach (var character in CharacterList)
					character.Write(writer);
			}
		}

		[Immutable]
		private class CharacterListEntry
		{
			public ObjectID Id { get; }
			public string Name { get; }
			public Race Race { get; }
			public Class Class { get; }
			public Sex Sex { get; }
			public byte Skin { get; }
			public byte Face { get; }
			public byte HairStyle { get; }
			public byte HairColor { get; }
			public byte FacialHair { get; }
			public byte Level { get; }
			public int ZoneId { get; }
			public int MapId { get; }
			public Vector3 Position { get; }
			public int GuildId { get; }
			public int PlayerFlags { get; }
			public bool IsFirstLogin { get; }
			public int PetDisplayId { get; }
			public int PetLevel { get; }
			public int PetFamily { get; }

			public IImmutableList<EquipmentSlotEntry> EquipmentSlots { get; }

			public int FirstBagDisplayId { get; }
			public byte FirstBagInventoryType { get; }

			public CharacterListEntry(ObjectID id, string name, Race race, Class @class, Sex sex, byte skin, byte face, byte hairStyle, byte hairColor, byte facialHair, byte level, int zoneId, int mapId, Vector3 position, int guildId, int playerFlags, bool isFirstLogin, int petDisplayId, int petLevel, int petFamily, IImmutableList<EquipmentSlotEntry> equipmentSlots, int firstBagDisplayId, byte firstBagInventoryType)
			{
				Id = id;
				Name = name;
				Race = race;
				Class = @class;
				Sex = sex;
				Skin = skin;
				Face = face;
				HairStyle = hairStyle;
				HairColor = hairColor;
				FacialHair = facialHair;
				Level = level;
				ZoneId = zoneId;
				MapId = mapId;
				Position = position;
				GuildId = guildId;
				PlayerFlags = playerFlags;
				IsFirstLogin = isFirstLogin;
				PetDisplayId = petDisplayId;
				PetLevel = petLevel;
				PetFamily = petFamily;
				EquipmentSlots = equipmentSlots;
				FirstBagDisplayId = firstBagDisplayId;
				FirstBagInventoryType = firstBagInventoryType;
			}

			public void Write(BinaryWriter writer)
			{
				writer.Write(Id);
				writer.WriteNullTerminatedString(Name, Encoding.UTF8);
				writer.Write((byte)Race);
				writer.Write((byte)Class);
				writer.Write((byte)Sex);
				writer.Write(Skin);
				writer.Write(Face);
				writer.Write(HairStyle);
				writer.Write(HairColor);
				writer.Write(FacialHair);
				writer.Write(Level);
				writer.Write(ZoneId);
				writer.Write(MapId);
				writer.Write(Position.X);
				writer.Write(Position.Y);
				writer.Write(Position.Z);
				writer.Write(GuildId);
				writer.Write(PlayerFlags);
				writer.Write(IsFirstLogin ? (byte)1 : (byte)0);
				writer.Write(PetDisplayId);
				writer.Write(PetLevel);
				writer.Write(PetFamily);

				foreach (var equipmentSlot in EquipmentSlots)
					equipmentSlot.Write(writer);

				writer.Write(FirstBagDisplayId);
				writer.Write(FirstBagInventoryType);
			}
		}

		[Immutable]
		private class EquipmentSlotEntry
		{
			public int DisplayId { get; }
			public byte InventoryType { get; }

			public EquipmentSlotEntry(int displayId, byte inventoryType)
			{
				DisplayId = displayId;
				InventoryType = inventoryType;
			}

			public void Write(BinaryWriter writer)
			{
				writer.Write(DisplayId);
				writer.Write(InventoryType);
			}
		}
	}
}
