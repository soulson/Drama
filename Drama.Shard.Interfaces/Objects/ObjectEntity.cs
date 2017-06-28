using Drama.Core.Interfaces;
using Drama.Core.Interfaces.Numerics;
using Newtonsoft.Json;
using System;

namespace Drama.Shard.Interfaces.Objects
{
	public class ObjectEntity
	{
		public ObjectEntity() : this((int)ObjectFields.END)
		{

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
		/// </summary>
		[JsonIgnore]
		private int[] Fields { get; }

		/// <summary>
		/// This mask keeps track of the fields that have been updated on this Object.
		/// It allows us not to send fields to the client if they have not changed since the last update.
		/// </summary>
		[JsonIgnore]
		public UpdateMask UpdateMask { get; }

		public bool Enabled { get; set; }
		public string Shard { get; set; }
		public Vector3 Position { get; set; }
		public float Orientation { get; set; }
		public int MapId { get; set; }

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

		protected uint GetFieldUnsigned(short index)
			=> unchecked((uint)Fields[index]);

		protected int GetFieldSigned(short index)
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
	}
}
