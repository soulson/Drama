﻿/* 
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
using Drama.Shard.Interfaces.WorldObjects;
using Newtonsoft.Json;

namespace Drama.Shard.Interfaces.Units
{
	/// <summary>
	/// Persisted storage for Unit grains.
	/// </summary>
	public class UnitEntity : WorldObjectEntity
	{
		/// <summary>
		/// Creates a new instance of the UnitEntity class.
		/// </summary>
		public UnitEntity() : this((short)UnitFields.END)
		{
			// persistent object entity public default constructors defer to a protected constructor
		}

		protected UnitEntity(int fieldCount) : base(fieldCount)
		{
			MoveTime = 0;
			MoveFlags = MovementFlags.None;
			MoveSpeed = new MovementSpeed();
			Jump = new Jump();

			TypeId = ObjectTypeID.Unit;
			CastSpeed = 1.0f;
			Energy = 100;
			EnergyMax = 100;
			RageMax = 1000;
			AttackTimeMainhandMilliseconds = 2000;
			AttackTimeOffhandMilliseconds = 2000;
			AttackTimeRangedMilliseconds = 2000;
		}
		
		public int MoveTime { get; set; }
		public int FallTime { get; set; }
		public float MovePitch { get; set; }
		public MovementFlags MoveFlags { get; set; }
		public MovementSpeed MoveSpeed { get; }
		public Jump Jump { get; }
		public float UnknownSplineThingy { get; set; }

		#region Transient Fields
		/// <summary>
		/// This field isn't part of the persisted state of a Unit. It is used to
		/// determine which move start/stop packets need to be sent to clients.
		/// </summary>
		[JsonIgnore]
		public MovementFlags PreviousMoveFlags { get; set; }

		/// <summary>
		/// This field isn't part of the persisted state of a Unit. It is used to
		/// determine whether a MoveSetOrientation packet needs to be sent to
		/// clients.
		/// </summary>
		[JsonIgnore]
		public float PreviousOrientation { get; set; }

		/// <summary>
		/// This field isn't part of the persisted state of a Unit. It is used to
		/// determine whether a MoveJump packet needs to be sent to clients.
		/// </summary>
		[JsonIgnore]
		public bool Jumped { get; set; }
		#endregion

		#region Client-visible State
		public int Health
		{
			get => GetFieldSigned(UnitFields.Health);
			set => SetField(UnitFields.Health, value);
		}

		public int HealthMax
		{
			get => GetFieldSigned(UnitFields.MaxHealth);
			set => SetField(UnitFields.MaxHealth, value);
		}

		public int HealthBase
		{
			get => GetFieldSigned(UnitFields.BaseHealth);
			set => SetField(UnitFields.BaseHealth, value);
		}

		public int Mana
		{
			get => GetFieldSigned(UnitFields.Mana);
			set => SetField(UnitFields.Mana, value);
		}

		public int ManaMax
		{
			get => GetFieldSigned(UnitFields.MaxMana);
			set => SetField(UnitFields.MaxMana, value);
		}

		public int ManaBase
		{
			get => GetFieldSigned(UnitFields.BaseMana);
			set => SetField(UnitFields.BaseMana, value);
		}

		public int Rage
		{
			get => GetFieldSigned(UnitFields.Rage);
			set => SetField(UnitFields.Rage, value);
		}

		public int RageMax
		{
			get => GetFieldSigned(UnitFields.MaxRage);
			set => SetField(UnitFields.MaxRage, value);
		}

		public int Energy
		{
			get => GetFieldSigned(UnitFields.Energy);
			set => SetField(UnitFields.Energy, value);
		}

		public int EnergyMax
		{
			get => GetFieldSigned(UnitFields.MaxEnergy);
			set => SetField(UnitFields.MaxEnergy, value);
		}

		public int Focus
		{
			get => GetFieldSigned(UnitFields.Focus);
			set => SetField(UnitFields.Focus, value);
		}

		public int FocusMax
		{
			get => GetFieldSigned(UnitFields.MaxFocus);
			set => SetField(UnitFields.MaxFocus, value);
		}

		public int Happiness
		{
			get => GetFieldSigned(UnitFields.Happiness);
			set => SetField(UnitFields.Happiness, value);
		}

		public int HappinessMax
		{
			get => GetFieldSigned(UnitFields.MaxHappiness);
			set => SetField(UnitFields.MaxHappiness, value);
		}

		public UnitFlags UnitFlags
		{
			get => (UnitFlags)GetFieldUnsigned(UnitFields.Flags);
			set => SetField(UnitFields.Flags, (uint)value);
		}

		public byte Level
		{
			get => (byte)GetFieldSigned(UnitFields.Level);
			set => SetField(UnitFields.Level, (int)value);
		}

		public int Strength
		{
			get => GetFieldSigned(UnitFields.Strength);
			set => SetField(UnitFields.Strength, value);
		}

		public int Agility
		{
			get => GetFieldSigned(UnitFields.Agility);
			set => SetField(UnitFields.Agility, value);
		}

		public int Stamina
		{
			get => GetFieldSigned(UnitFields.Stamina);
			set => SetField(UnitFields.Stamina, value);
		}

		public int Intellect
		{
			get => GetFieldSigned(UnitFields.Intellect);
			set => SetField(UnitFields.Intellect, value);
		}

		public int Spirit
		{
			get => GetFieldSigned(UnitFields.Spirit);
			set => SetField(UnitFields.Spirit, value);
		}

		public int DisplayID
		{
			get => GetFieldSigned(UnitFields.DisplayID);
			set => SetField(UnitFields.DisplayID, value);
		}

		public int NativeDisplayID
		{
			get => GetFieldSigned(UnitFields.NativeDisplayID);
			set => SetField(UnitFields.NativeDisplayID, value);
		}

		public int FactionTemplate
		{
			get => GetFieldSigned(UnitFields.FactionTemplate);
			set => SetField(UnitFields.FactionTemplate, value);
		}

		public int AttackTimeMainhandMilliseconds
		{
			get => GetFieldSigned(UnitFields.AttackTimeBase);
			set => SetField(UnitFields.AttackTimeBase, value);
		}

		public int AttackTimeRangedMilliseconds
		{
			get => GetFieldSigned(UnitFields.AttackTimeRanged);
			set => SetField(UnitFields.AttackTimeRanged, value);
		}

		public int AttackTimeOffhandMilliseconds
		{
			get => GetFieldSigned(UnitFields.AttackTimeOffhand);
			set => SetField(UnitFields.AttackTimeOffhand, value);
		}

		public int AttackPower
		{
			get => GetFieldSigned(UnitFields.AttackPower);
			set => SetField(UnitFields.AttackPower, value);
		}

		public int AttackPowerRanged
		{
			get => GetFieldSigned(UnitFields.AttackPowerRanged);
			set => SetField(UnitFields.AttackPowerRanged, value);
		}

		public float BoundingRadius
		{
			get => GetFieldFloat(UnitFields.BoundingRadius);
			set => SetField(UnitFields.BoundingRadius, value);
		}

		public float CombatReach
		{
			get => GetFieldFloat(UnitFields.CombatReach);
			set => SetField(UnitFields.CombatReach, value);
		}

		public float AttackDamageMainhandMin
		{
			get => GetFieldFloat(UnitFields.DamageMin);
			set => SetField(UnitFields.DamageMin, value);
		}

		public float AttackDamageMainhandMax
		{
			get => GetFieldFloat(UnitFields.DamageMax);
			set => SetField(UnitFields.DamageMax, value);
		}

		public float AttackDamageRangedMin
		{
			get => GetFieldFloat(UnitFields.DamageRangedMin);
			set => SetField(UnitFields.DamageRangedMin, value);
		}

		public float AttackDamageRangedMax
		{
			get => GetFieldFloat(UnitFields.DamageRangedMax);
			set => SetField(UnitFields.DamageRangedMax, value);
		}

		public float CastSpeed
		{
			get => GetFieldFloat(UnitFields.ModCastSpeed);
			set => SetField(UnitFields.ModCastSpeed, value);
		}

		public int Armor
		{
			get => GetFieldSigned(UnitFields.Armor);
			set => SetField(UnitFields.Armor, value);
		}

		public Race Race
		{
			get => (Race)GetFieldByte((short)UnitFields.Bytes0, 0);
			set => SetField((short)UnitFields.Bytes0, 0, (byte)value);
		}

		public Class Class
		{
			get => (Class)GetFieldByte((short)UnitFields.Bytes0, 1);
			set => SetField((short)UnitFields.Bytes0, 1, (byte)value);
		}

		/// <remarks>
		/// Character stores Sex in an additional location, so it needs to override this.
		/// </remarks>
		public virtual Sex Sex
		{
			get => (Sex)GetFieldByte((short)UnitFields.Bytes0, 2);
			set => SetField((short)UnitFields.Bytes0, 2, (byte)value);
		}

		public Resource ActiveResource
		{
			get => (Resource)GetFieldByte((short)UnitFields.Bytes0, 3);
			set => SetField((short)UnitFields.Bytes0, 3, (byte)value);
		}

		public StandState StandState
		{
			get => (StandState)GetFieldByte((short)UnitFields.Bytes1, 0);
			set => SetField((short)UnitFields.Bytes1, 0, (byte)value);
		}

		/// <remarks>
		/// For some reason, this is set to 0xee for players, but doesn't seem to be necessary.
		/// </remarks>
		public Loyalty Loyalty
		{
			get => (Loyalty)GetFieldByte((short)UnitFields.Bytes1, 1);
			set => SetField((short)UnitFields.Bytes1, 1, (byte)value);
		}

		public Form Form
		{
			get => (Form)GetFieldByte((short)UnitFields.Bytes1, 2);
			set => SetField((short)UnitFields.Bytes1, 2, (byte)value);
		}

		public UnitFlags1 UnitFlags1
		{
			get => (UnitFlags1)GetFieldByte((short)UnitFields.Bytes1, 3);
			set => SetField((short)UnitFields.Bytes1, 3, (byte)value);
		}

		public SheathState SheathState
		{
			get => (SheathState)GetFieldByte((short)UnitFields.Bytes2, 0);
			set => SetField((short)UnitFields.Bytes2, 0, (byte)value);
		}

		public UnitFlags2 UnitFlags2
		{
			get => (UnitFlags2)GetFieldByte((short)UnitFields.Bytes2, 1);
			set => SetField((short)UnitFields.Bytes2, 1, (byte)value);
		}

		public ObjectID TargetId
		{
			get => new ObjectID(GetFieldLong(UnitFields.Target));
			set => SetField(UnitFields.Target, value);
		}
		#endregion

		#region Field Management
		public uint GetFieldUnsigned(UnitFields field)
			=> GetFieldUnsigned((short)field);

		public int GetFieldSigned(UnitFields field)
			=> GetFieldSigned((short)field);

		public float GetFieldFloat(UnitFields field)
			=> GetFieldFloat((short)field);

		public long GetFieldLong(UnitFields field)
			=> GetFieldLong((short)field);

		public void SetField(UnitFields field, uint value)
			=> SetField((short)field, value);

		public void SetField(UnitFields field, int value)
			=> SetField((short)field, value);

		public void SetField(UnitFields field, float value)
			=> SetField((short)field, value);

		public void SetField(UnitFields field, long value)
			=> SetField((short)field, value);
		#endregion
	}
}
