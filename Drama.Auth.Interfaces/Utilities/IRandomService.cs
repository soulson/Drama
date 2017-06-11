using Orleans;
using System;
using System.Numerics;
using System.Threading.Tasks;

namespace Drama.Auth.Interfaces.Utilities
{
	public interface IRandomService : IGrainWithIntegerKey
	{
		Task<BigInteger> GetRandomBigInteger(int sizeInBytes);
		Task<byte[]> GetRandomBytes(int size);
		Task<int> GetRandomInt();
		Task<int> GetRandomInt(int exclusiveUpperBound);
	}
}
