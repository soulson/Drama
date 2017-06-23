using System;
using System.IO;
using System.Numerics;

namespace Drama.Core.Interfaces.Utilities
{
	public static class BigIntegers
	{
		public static BigInteger FromUnsignedByteArray(byte[] input)
		{
			// sign bit
			if ((input[input.Length - 1] & 0x80) != 0)
			{
				byte[] padded = new byte[input.Length + 1];
				Array.Copy(input, padded, input.Length);
				padded[input.Length] = 0;
				return new BigInteger(padded);
			}
			else
				return new BigInteger(input);
		}

		public static byte[] ToByteArray(this BigInteger value, int length)
			=> Arrays.Left(value.ToByteArray(), length);

		public static BigInteger ReadBigInteger(this BinaryReader reader, int sizeInBytes)
			=> FromUnsignedByteArray(reader.ReadBytes(sizeInBytes));
	}
}
