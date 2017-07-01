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

using Drama.Shard.Interfaces.Objects;
using Drama.Shard.Interfaces.Units;

namespace Drama.Shard.Interfaces.Characters
{
	public class CharacterEntity : UnitEntity
	{
		public CharacterEntity() : this((short)CharacterFields.END)
		{

		}

		protected CharacterEntity(short fieldCount) : base(fieldCount)
		{
			TypeId = ObjectTypeID.Player;

			DamageDoneArcaneMultiplier = 1.0f;
			DamageDoneFrostMultiplier = 1.0f;
			DamageDoneFireMultiplier = 1.0f;
			DamageDoneNatureMultiplier = 1.0f;
			DamageDoneShadowMultiplier = 1.0f;
			DamageDoneHolyMultiplier = 1.0f;
			DamageDonePhysicalMultiplier = 1.0f;

			UnitFlags |= UnitFlags.PvPUnit;

			// TODO: instead of 0x18 as described here, this field has value 0x28 for players. need to determine the difference
			UnitFlags2 |= UnitFlags2.Supportable;
			UnitFlags2 |= UnitFlags2.CanHaveAuras;

			RestState = RestState.Normal;
			WatchedFactionIndex = -1;

			// TODO: placeholder values
			BoundingRadius = 1.0f * Scale;
			CombatReach = 1.0f * Scale;

			XPNextLevel = 400;
			HealthMax = Health = HealthBase = 100;
			ManaMax = Mana = ManaBase = 100;
			Armor = 100;
			Strength = 1;
			Agility = 1;
			Stamina = 1;
			Intellect = 1;
			Spirit = 1;
		}

		public string Name { get; set; }
		public string Account { get; set; }
		public int ZoneId { get; set; }

		#region Client-visible State
		public byte Skin
		{
			get => GetFieldByte((short)CharacterFields.Bytes1, 0);
			set => SetField((short)CharacterFields.Bytes1, 0, value);
		}

		public byte Face
		{
			get => GetFieldByte((short)CharacterFields.Bytes1, 1);
			set => SetField((short)CharacterFields.Bytes1, 1, value);
		}

		public byte HairStyle
		{
			get => GetFieldByte((short)CharacterFields.Bytes1, 2);
			set => SetField((short)CharacterFields.Bytes1, 2, value);
		}

		public byte HairColor
		{
			get => GetFieldByte((short)CharacterFields.Bytes1, 3);
			set => SetField((short)CharacterFields.Bytes1, 3, value);
		}

		public byte FacialHair
		{
			get => GetFieldByte((short)CharacterFields.Bytes2, 0);
			set => SetField((short)CharacterFields.Bytes2, 0, value);
		}

		public RestState RestState
		{
			get => (RestState)GetFieldByte((short)CharacterFields.Bytes2, 3);
			set => SetField((short)CharacterFields.Bytes2, 3, (byte)value);
		}

		public float DamageDonePhysicalMultiplier
		{
			get => GetFieldFloat(CharacterFields.DamageDonePhysicalModPercent);
			set => SetField(CharacterFields.DamageDonePhysicalModPercent, value);
		}

		public float DamageDoneHolyMultiplier
		{
			get => GetFieldFloat(CharacterFields.DamageDoneHolyModPercent);
			set => SetField(CharacterFields.DamageDoneHolyModPercent, value);
		}

		public float DamageDoneFireMultiplier
		{
			get => GetFieldFloat(CharacterFields.DamageDoneFireModPercent);
			set => SetField(CharacterFields.DamageDoneFireModPercent, value);
		}

		public float DamageDoneNatureMultiplier
		{
			get => GetFieldFloat(CharacterFields.DamageDoneNatureModPercent);
			set => SetField(CharacterFields.DamageDoneNatureModPercent, value);
		}

		public float DamageDoneFrostMultiplier
		{
			get => GetFieldFloat(CharacterFields.DamageDoneFrostModPercent);
			set => SetField(CharacterFields.DamageDoneFrostModPercent, value);
		}

		public float DamageDoneShadowMultiplier
		{
			get => GetFieldFloat(CharacterFields.DamageDoneShadowModPercent);
			set => SetField(CharacterFields.DamageDoneShadowModPercent, value);
		}

		public float DamageDoneArcaneMultiplier
		{
			get => GetFieldFloat(CharacterFields.DamageDoneArcaneModPercent);
			set => SetField(CharacterFields.DamageDoneArcaneModPercent, value);
		}

		public int WatchedFactionIndex
		{
			get => GetFieldSigned(CharacterFields.WatchedFactionIndex);
			set => SetField(CharacterFields.WatchedFactionIndex, value);
		}

		public int XPNextLevel
		{
			get => GetFieldSigned(CharacterFields.XPNextLevel);
			set => SetField(CharacterFields.XPNextLevel, value);
		}

		public float CritChanceMelee
		{
			get => GetFieldFloat(CharacterFields.CritPercent);
			set => SetField(CharacterFields.CritPercent, value);
		}

		public float CritChanceRanged
		{
			get => GetFieldFloat(CharacterFields.CritPercentRanged);
			set => SetField(CharacterFields.CritPercentRanged, value);
		}

		public float DodgeChance
		{
			get => GetFieldFloat(CharacterFields.DodgePercent);
			set => SetField(CharacterFields.DodgePercent, value);
		}

		public override Sex Sex
		{
			get => base.Sex;
			set
			{
				// who's to say why Characters store this field twice? /shrug
				SetField((short)CharacterFields.Bytes3, 0, (byte)Sex);
				base.Sex = value;
			}
		}
		#endregion

		#region Field Management
		public uint GetFieldUnsigned(CharacterFields field)
			=> GetFieldUnsigned((short)field);

		public int GetFieldSigned(CharacterFields field)
			=> GetFieldSigned((short)field);

		public float GetFieldFloat(CharacterFields field)
			=> GetFieldFloat((short)field);

		public void SetField(CharacterFields field, uint value)
			=> SetField((short)field, value);

		public void SetField(CharacterFields field, int value)
			=> SetField((short)field, value);

		public void SetField(CharacterFields field, float value)
			=> SetField((short)field, value);
		#endregion
	}
}
