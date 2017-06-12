using Orleans;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Drama.Auth.Interfaces.Shard
{
	public interface IShardList : IGrainWithIntegerKey
	{
		Task<IList<string>> GetShardKeys();
		Task AddShardKey(string key);
	}
}
