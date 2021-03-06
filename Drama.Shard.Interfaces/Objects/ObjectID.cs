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

using Drama.Core.Interfaces.Utilities;
using Newtonsoft.Json;
using System;

namespace Drama.Shard.Interfaces.Objects
{
	/// <summary>
	/// An ObjectID is used to uniquely identify a PersistentObject in the game
	/// world.
	/// </summary>
	[JsonObject(MemberSerialization.Fields)]
	public struct ObjectID : IEquatable<ObjectID>
	{
		private const ulong TypeMask = 0xffff000000000000;

		private ulong id;

		public ObjectID(ulong longForm)
			=> id = longForm;

		public ObjectID(long longForm)
			=> id = unchecked((ulong)longForm);

		public ObjectID(ulong id, Type type)
			=> this.id = (id & ~TypeMask) | ((ulong)type & TypeMask);

		public ObjectID(long id, Type type)
			=> this.id = (checked((ulong)id) & ~TypeMask) | ((ulong)type & TypeMask);

		public ObjectID(int id, Type type)
			=> this.id = unchecked((uint)id) | (ulong)type;

		public int Id
		{
			get => checked((int)(id & ~TypeMask));
			set => id = (id & TypeMask) | (checked((ulong)value) & ~TypeMask);
		}

		public Type ObjectType
		{
			get => (Type)(id & TypeMask);
			set
			{
				id = id & ~TypeMask;
				id |= (ulong)value & TypeMask;
			}
		}

		[JsonIgnore]
		public uint MaskId => (uint)((id & TypeMask) >> 48);

		public byte[] GetPacked()
		{
			ulong id = this.id;
			var bytes = new byte[9];
			var size = 1;

			for (var i = 0; id != 0 && size < 9; ++i)
			{
				if ((id & 0xff) != 0)
				{
					bytes[0] |= (byte)(1 << i);
					bytes[size] = (byte)(id & 0xff);

					++size;
				}

				id >>= 8;
			}

			return Arrays.Left(bytes, size);
		}

		public override string ToString()
			=> $"0x{id:x16}";

		public override bool Equals(object obj)
		{
			if (obj == null)
				return false;
			else if (obj.GetType().Equals(GetType()))
				return Equals((ObjectID)obj);
			else
				return false;
		}

		public override int GetHashCode()
			=> Id ^ ((int)ObjectType >> 32);

		public bool Equals(ObjectID other)
			=> id == other.id;

		public static ObjectID Null { get; } = new ObjectID(0);

		public static bool operator ==(ObjectID left, ObjectID right)
			=> left.Equals(right);

		public static bool operator !=(ObjectID left, ObjectID right)
			=> !left.Equals(right);

		public static implicit operator long(ObjectID value)
			=> unchecked((long)value.id);

		public enum Type : ulong
		{
			Item = 0x4000000000000000,
			Container = 0x4000000000000000,
			Player = 0x0000000000000000,
			GameObject = 0xf110000000000000,
			Transport = 0xf120000000000000,
			Unit = 0xf130000000000000,
			Pet = 0xf140000000000000,
			DynamicObject = 0xf100000000000000,
			Corpse = 0xf101000000000000,
			MoTransport = 0x1fc0000000000000,
		}
	}
}
