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

using Drama.Auth.Interfaces.Utilities;
using Drama.Core.Interfaces.Utilities;
using Orleans;
using Orleans.Concurrency;
using System;
using System.Numerics;
using System.Security.Cryptography;
using System.Threading.Tasks;

namespace Drama.Auth.Grains.Utilities
{
	// stateless so each silo can have its own activation. however, there can be only one per silo
	//  to eliminate the possibility of repeated-seed patterns in the random results
	[StatelessWorker(1)]
	public sealed class RandomService : Grain, IRandomService, IDisposable
	{
		private readonly RandomNumberGenerator randomGenerator;

		public RandomService() => randomGenerator = RandomNumberGenerator.Create();

		public Task<BigInteger> GetRandomBigInteger(int sizeInBytes)
		{
			if (sizeInBytes < 1)
				throw new ArgumentOutOfRangeException(nameof(sizeInBytes), "must be greater than 0");

			return Task.FromResult(BigIntegers.FromUnsignedByteArray(GetRandomBytes(sizeInBytes).Result));
		}

		public Task<byte[]> GetRandomBytes(int size)
		{
			var bytes = new byte[size];
			randomGenerator.GetBytes(bytes);
			return Task.FromResult(bytes);
		}

		public Task<int> GetRandomInt() => Task.FromResult(BitConverter.ToInt32(GetRandomBytes(sizeof(int)).Result, 0));

		public Task<int> GetRandomInt(int exclusiveUpperBound)
		{
			if (exclusiveUpperBound < 1)
				throw new ArgumentOutOfRangeException(nameof(exclusiveUpperBound), "must be greater than 0");

			// the cast to long here is to make sure Abs doesn't crash if GetRandomInt returns int.MinValue
			return Task.FromResult((int)(Math.Abs((long)GetRandomInt().Result) % exclusiveUpperBound));
		}

		public void Dispose() => randomGenerator.Dispose();
	}
}
