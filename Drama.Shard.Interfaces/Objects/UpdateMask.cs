using System;

namespace Drama.Shard.Interfaces.Objects
{
	public class UpdateMask
	{
		public UpdateMask(int valueCount)
		{
			ValueCount = valueCount;
			BlockCount = (byte)((valueCount + 31) / 32);
			Data = new byte[BlockCount * 4];
		}

		public int ValueCount { get; }
		public byte BlockCount { get; }
		public byte[] Data { get; }

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

		public void Clear()
		{
			for (int i = 0; i < Data.Length; ++i)
				Data[i] = 0;
		}

		public void SetBit(int index)
			=> Data[index >> 3] |= (byte)(1 << (index & 0x7));

		public void UnsetBit(int index)
			=> Data[index >> 3] &= (byte)~(1 << (index & 0x7));

		public bool GetBit(int index)
			=> (Data[index >> 3] & (1 << (index & 0x7))) != 0;
	}
}
