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

using Drama.Core.Interfaces;
using Newtonsoft.Json;
using System;

namespace Drama.Shard.Interfaces.Objects
{
	/// <summary>
	/// Persisted storage for Object grains.
	/// </summary>
	public class ObjectEntity
	{
		/// <summary>
		/// Creates a new instance of the ObjectEntity class.
		/// </summary>
		public ObjectEntity() : this((int)ObjectFields.END)
		{
			// object entity public default constructors defer to a protected constructor
		}

		protected ObjectEntity(int fieldCount)
		{
			Fields = new int[fieldCount];
			UpdateMask = new UpdateMask(fieldCount);
			TypeId = ObjectTypeID.Object;
			Scale = 1.0f;
		}
		
		/// <summary>
		/// This array stores the client-visible state of an Object.
		/// Values in the ObjectFields enum are indexes into this array.
		/// This array should not be written into directly; use the Set methods or properties to do so.
		/// </summary>
		[JsonIgnore]
		private int[] Fields { get; }

		/// <summary>
		/// This mask keeps track of the fields that have been updated on this Object.
		/// It allows us not to send fields to the client if they have not changed since the last update.
		/// </summary>
		[JsonIgnore]
		public UpdateMask UpdateMask { get; }

		/// <summary>
		/// This mask keeps track of all non-zero fields on this Object.
		/// It allows us to not send packets full of zeroes when creating a new object client-side.
		/// </summary>
		[JsonIgnore]
		public UpdateMask CreateMask
		{
			get
			{
				var mask = new UpdateMask(Fields.Length);

				for(int i = 0; i < Fields.Length; ++i)
				{
					if (Fields[i] != 0)
						mask.SetBit(i);
				}

				return mask;
			}
		}

		/// <summary>
		/// True if this object has been created, false otherwise.
		/// </summary>
		public bool Enabled { get; set; }

		/// <summary>
		/// The name of the shard on which this object exists.
		/// </summary>
		public string Shard { get; set; }

		#region Client-visible State
		public ObjectID Id
		{
			get => new ObjectID(GetFieldLong(ObjectFields.Id));
			set => SetField(ObjectFields.Id, value);
		}
		
		public ObjectTypeID TypeId
		{
			get
			{
				var typeMask = (ObjectTypeMask)GetFieldUnsigned(ObjectFields.Type);

				if (typeMask.HasFlag(ObjectTypeMask.Player))
					return ObjectTypeID.Player;
				if (typeMask.HasFlag(ObjectTypeMask.Unit))
					return ObjectTypeID.Unit;
				if (typeMask.HasFlag(ObjectTypeMask.Object))
					return ObjectTypeID.Object;

				throw new DramaException($"illegal {nameof(ObjectTypeMask)} value {typeMask}");
			}
			set
			{
				ObjectTypeMask typeMask = ObjectTypeMask.None;
				switch (value)
				{
					case ObjectTypeID.Player:
						typeMask |= ObjectTypeMask.Player;
						goto case ObjectTypeID.Unit;

					case ObjectTypeID.Unit:
						typeMask |= ObjectTypeMask.Unit;
						goto case ObjectTypeID.Object;

					case ObjectTypeID.Object:
						typeMask |= ObjectTypeMask.Object;
						break;

					default:
						throw new ArgumentException($"illegal {nameof(ObjectTypeID)} value {value}");
				}

				SetField(ObjectFields.Type, (uint)typeMask);
			}
		}

		public float Scale
		{
			get => GetFieldFloat(ObjectFields.Scale);
			set => SetField(ObjectFields.Scale, value);
		}

		public int Entry
		{
			get => GetFieldSigned(ObjectFields.Entry);
			set => SetField(ObjectFields.Entry, value);
		}
		#endregion

		#region Field Management
		protected uint GetFieldUnsigned(short index)
			=> unchecked((uint)Fields[index]);

		public int GetFieldSigned(short index)
			=> Fields[index];

		protected long GetFieldLong(short index)
			=> unchecked((uint)Fields[index] | ((long)Fields[index + 1] << 32));

		protected byte GetFieldByte(short fieldIndex, byte byteIndex)
		{
			uint bytes = GetFieldUnsigned(fieldIndex);
			bytes &= (uint)0x000000ff << (byteIndex << 3);
			return (byte)(bytes >> (byteIndex << 3));
		}

		protected float GetFieldFloat(short index)
			=> BitConverter.ToSingle(BitConverter.GetBytes(GetFieldSigned(index)), 0);

		protected void SetField(short index, uint value)
		{
			if (Fields[index] != value)
			{
				UpdateMask.SetBit(index);
				Fields[index] = unchecked((int)value);
			}
		}

		protected void SetField(short index, long value)
		{
			UpdateMask.SetBit(index);
			UpdateMask.SetBit(index + 1);

			unchecked
			{
				Fields[index] = (int)(value & 0xffffffff);
				Fields[index + 1] = (int)(value >> 32);
			}
		}

		protected void SetField(short index, int value)
		{
			if (Fields[index] != value)
			{
				UpdateMask.SetBit(index);
				Fields[index] = value;
			}
		}

		protected void SetField(short fieldIndex, byte byteIndex, byte value)
		{
			UpdateMask.SetBit(fieldIndex);

			uint bytes = GetFieldUnsigned(fieldIndex);
			bytes &= ~((uint)0x000000ff << (byteIndex << 3));
			SetField(fieldIndex, bytes | ((uint)value << (byteIndex << 3)));
		}

		protected void SetField(short index, float value)
		{
			int intVal = BitConverter.ToInt32(BitConverter.GetBytes(value), 0);
			if (Fields[index] != intVal)
			{
				UpdateMask.SetBit(index);
				Fields[index] = intVal;
			}
		}

		public uint GetFieldUnsigned(ObjectFields field)
			=> GetFieldUnsigned((short)field);

		public int GetFieldSigned(ObjectFields field)
			=> GetFieldSigned((short)field);

		public long GetFieldLong(ObjectFields field)
			=> GetFieldLong((short)field);

		public float GetFieldFloat(ObjectFields field)
			=> GetFieldFloat((short)field);

		public void SetField(ObjectFields field, uint value)
			=> SetField((short)field, value);

		public void SetField(ObjectFields field, int value)
			=> SetField((short)field, value);

		public void SetField(ObjectFields field, long value)
			=> SetField((short)field, value);

		public void SetField(ObjectFields field, float value)
			=> SetField((short)field, value);
		#endregion

		public override bool Equals(object obj)
		{
			if (!(obj is ObjectEntity))
				return false;
			else
				return Id == (obj as ObjectEntity).Id;
		}

		public override int GetHashCode()
		  => Id.Id;
	}
}
