using Orleans;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Drama.Auth.Interfaces.Shard
{
	public interface IShardList : IGrainWithIntegerKey
	{
		Task AddShardKey(string key);
		Task<IList<string>> GetShardKeys();
	}
}
