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

using System;

namespace Drama.Shard.Interfaces.Objects
{
	/// <summary>
	/// UpdateMasks keep track of which fields on a PersistentObject have
	/// changed, so that only the set of changed fields needs to be seent to the
	/// client on each update.
	/// </summary>
	public class UpdateMask
	{
		public UpdateMask(int valueCount)
		{
			ValueCount = valueCount;
			BlockCount = (byte)((valueCount + 31) / 32);
			Data = new byte[BlockCount * 4];
		}

		/// <summary>
		/// The number of fields that this UpdateMask is tracking.
		/// </summary>
		public int ValueCount { get; }

		/// <summary>
		/// The number of 4-byte blocks that make up this UpdateMask's Data field.
		/// </summary>
		public byte BlockCount { get; }

		/// <summary>
		/// A bit vector representing which fields have been updated.
		/// </summary>
		public byte[] Data { get; }

		/// <summary>
		/// True if there are no set bits in this UpdateMask; that is, if
		/// ActiveBitCount is zero.
		/// </summary>
		public bool IsEmpty
		{
			get
			{
				foreach (byte b in Data)
				{
					if (b != 0)
						return false;
				}

				return true;
			}
		}

		/// <summary>
		/// The number of set bits in this UpdateMask.
		/// </summary>
		public int ActiveBitCount
		{
			get
			{
				int count = 0;

				foreach(byte b in Data)
				{
					for (int x = b; x != 0; count++)
						x &= x - 1;
				}

				return count;
			}
		}

		/// <summary>
		/// Sets all bits in this UpdateMask to zero.
		/// </summary>
		public void Clear()
		{
			for (int i = 0; i < Data.Length; ++i)
				Data[i] = 0;
		}

		/// <summary>
		/// Sets the bit representing the field at index to one.
		/// </summary>
		public void SetBit(int index)
			=> Data[index >> 3] |= (byte)(1 << (index & 0x7));

		/// <summary>
		/// Sets the bit representing the field at index to zero.
		/// </summary>
		public void UnsetBit(int index)
			=> Data[index >> 3] &= (byte)~(1 << (index & 0x7));

		/// <summary>
		/// Gets the value of the bit at index.
		/// </summary>
		public bool GetBit(int index)
			=> (Data[index >> 3] & (1 << (index & 0x7))) != 0;
	}
}
