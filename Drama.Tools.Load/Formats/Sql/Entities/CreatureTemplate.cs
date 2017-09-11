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
using Drama.Shard.Interfaces.Creatures;
using Drama.Shard.Interfaces.Spells;
using Drama.Shard.Interfaces.Units;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace Drama.Tools.Load.Formats.Sql.Entities
{
	[Table("creature_template")]
	public class CreatureTemplate : ISqlEntity<CreatureDefinitionEntity>
	{
		[Column("entry")]
		public int Id { get; set; }

		[Column("modelid_1")]
		public int ModelId1 { get; set; }
		[Column("modelid_2")]
		public int ModelId2 { get; set; }
		[Column("modelid_3")]
		public int ModelId3 { get; set; }
		[Column("modelid_4")]
		public int ModelId4 { get; set; }

		[Column("name")]
		public string Name { get; set; }
		[Column("subname")]
		public string Subname { get; set; }

		[Column("minlevel")]
		public byte LevelMin { get; set; }
		[Column("maxlevel")]
		public byte LevelMax { get; set; }

		[Column("minhealth")]
		public int HealthMin { get; set; }
		[Column("maxhealth")]
		public int HealthMax { get; set; }

		[Column("minmana")]
		public int ManaMin { get; set; }
		[Column("maxmana")]
		public int ManaMax { get; set; }

		[Column("armor")]
		public int Armor { get; set; }
		[Column("npcflag")]
		public NpcFlags NpcFlags { get; set; }
		[Column("speed_walk")]
		public float SpeedWalking { get; set; }
		[Column("speed_run")]
		public float SpeedRunning { get; set; }
		[Column("scale")]
		public float Scale { get; set; }

		[Column("mindmg")]
		public float AttackDamageMin { get; set; }
		[Column("maxdmg")]
		public float AttackDamageMax { get; set; }

		[Column("minrangedmg")]
		public float AttackDamageRangedMin { get; set; }
		[Column("maxrangedmg")]
		public float AttackDamageRangedMax { get; set; }

		[Column("dmgschool")]
		public School AttackSchool { get; set; }
		[Column("attackpower")]
		public int AttackPower { get; set; }
		[Column("rangedattackpower")]
		public int AttackPowerRanged { get; set; }
		[Column("baseattacktime")]
		public int AttackTimeMilliseconds { get; set; }
		[Column("rangeattacktime")]
		public int AttackTimeRangedMilliseconds { get; set; }
		[Column("dmg_multiplier")]
		public float DamageMultiplier { get; set; }
		[Column("unit_class")]
		public Class Class { get; set; }
		[Column("unit_flags")]
		public UnitFlags UnitFlags { get; set; }

		[Column("faction_A")]
		public int FactionIdAlliance { get; set; }
		[Column("faction_H")]
		public int FactionIdHorde { get; set; }

		[Column("rank")]
		public CreatureRank Rank { get; set; }
		[Column("family")]
		public CreatureFamily Family { get; set; }
		[Column("type")]
		public CreatureType Type { get; set; }
		[Column("type_flags")]
		public CreatureFlags CreatureFlags { get; set; }

		[Column("PetSpellDataId")]
		public int PetSpellDataId { get; set; }
		[Column("Civilian")]
		public bool Civilian { get; set; }

		[Column("equipment_id")]
		public int EquipmentId { get; set; }

		public long GetKey()
			=> Id;

		public CreatureDefinitionEntity ToGrainEntity()
		{
			var grainEntity = new CreatureDefinitionEntity
			{
				Exists = true,
				Id = Id,
				Name = Name,
				Subname = Subname,
				Level = CheckRange(LevelMin, LevelMax),
				Health = CheckRange(HealthMin, HealthMax),
				Mana = CheckRange(ManaMin, ManaMax),
				Armor = Armor,
				NpcFlags = NpcFlags,
				SpeedWalking = SpeedWalking,
				SpeedRunning = SpeedRunning,
				Scale = Scale,
				AttackDamage = CheckRange(AttackDamageMin, AttackDamageMax),
				AttackDamageRanged = CheckRange(AttackDamageRangedMin, AttackDamageRangedMax),
				AttackSchool = AttackSchool,
				AttackPower = AttackPower,
				AttackPowerRanged = AttackPowerRanged,
				AttackTimeMilliseconds = AttackTimeMilliseconds,
				AttackTimeRangedMilliseconds = AttackTimeRangedMilliseconds,
				DamageMultiplier = DamageMultiplier,
				Class = Class,
				UnitFlags = UnitFlags,
				Civilian = Civilian,
				CreatureFlags = CreatureFlags,
				Family = Family,
				PetSpellDataId = PetSpellDataId,
				Rank = Rank,
				Type = Type,
			};

			grainEntity.ModelIds = new SortedSet<int>();

			if (ModelId1 != 0)
				grainEntity.ModelIds.Add(ModelId1);
			if (ModelId2 != 0)
				grainEntity.ModelIds.Add(ModelId2);
			if (ModelId3 != 0)
				grainEntity.ModelIds.Add(ModelId3);
			if (ModelId4 != 0)
				grainEntity.ModelIds.Add(ModelId4);

			return grainEntity;
		}

		private Range<T> CheckRange<T>(T min, T max) where T : struct, IComparable<T>
		{
			if (min.CompareTo(max) > 0)
			{
				Console.Error.WriteLine($"### WARN creature {Id} \"{Name}\" has bad int range with min > max. using [min, min]");
				return new Range<T>(min, min);
			}

			return new Range<T>(min, max);
		}
	}
}
