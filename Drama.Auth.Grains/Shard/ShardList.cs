using Drama.Auth.Interfaces.Shard;
using Orleans;
using Orleans.Providers;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Drama.Auth.Grains.Shard
{
	[StorageProvider(ProviderName = "DeploymentStore")]
	public class ShardList : Grain<List<string>>, IShardList
	{
		public Task AddShardKey(string key)
		{
			if (!State.Contains(key))
				State.Add(key);

			return WriteStateAsync();
		}

		public Task<IList<string>> GetShardKeys() => Task.FromResult<IList<string>>(State);
	}
}
