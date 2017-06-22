﻿using System;
using System.Collections.Generic;
using System.Numerics;
using System.Security.Cryptography;
using System.Text;

namespace Drama.Core.Interfaces.Security
{
	public class Digester : IDisposable
	{
		private readonly HashAlgorithm algorithm;

		public Digester(HashAlgorithm hashAlgorithm)
		{
			algorithm = hashAlgorithm;
		}

		public int DigestSize => algorithm.HashSize / 8;

		public byte[] CalculateDigest(byte[][] input)
		{
			int totalSize = 0;
			foreach (byte[] array in input)
				totalSize += array.Length;

			int index = 0;
			byte[] final = new byte[totalSize];
			foreach (byte[] array in input)
			{
				Array.Copy(array, 0, final, index, array.Length);
				index += array.Length;
			}

			return CalculateDigest(final);
		}

		public byte[] CalculateDigest(byte[] input)
		{
			algorithm.Initialize();
			return algorithm.ComputeHash(input, 0, input.Length);
		}

		public byte[] CalculateDigest(String message, Encoding encoding)
		{
			byte[] input = encoding.GetBytes(message);
			return CalculateDigest(input);
		}

		public byte[] CalculateDigest(String message)
		{
			return CalculateDigest(message, Encoding.UTF8);
		}

		// this is dangerous to use, since if the BigInteger is smaller than expected, it does not pad with 00 bytes. make sure you know what you're doing
		public byte[] CalculateDigest(params BigInteger[] bigInts)
		{
			List<byte> bytes = new List<byte>();

			foreach (BigInteger bigInt in bigInts)
				bytes.AddRange(bigInt.ToByteArray());

			return CalculateDigest(bytes.ToArray());
		}

		#region IDisposable Support
		private bool disposedValue = false; // To detect redundant calls

		protected virtual void Dispose(bool disposing)
		{
			if (!disposedValue)
			{
				if (disposing)
				{
					// TODO: dispose managed state (managed objects).
					algorithm.Dispose();
				}

				// TODO: free unmanaged resources (unmanaged objects) and override a finalizer below.
				// TODO: set large fields to null.

				disposedValue = true;
			}
		}

		// TODO: override a finalizer only if Dispose(bool disposing) above has code to free unmanaged resources.
		// ~Digester() {
		//   // Do not change this code. Put cleanup code in Dispose(bool disposing) above.
		//   Dispose(false);
		// }

		// This code added to correctly implement the disposable pattern.
		public void Dispose()
		{
			// Do not change this code. Put cleanup code in Dispose(bool disposing) above.
			Dispose(true);
			// TODO: uncomment the following line if the finalizer is overridden above.
			// GC.SuppressFinalize(this);
		}
		#endregion
	}
}