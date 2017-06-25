using Drama.Auth.Interfaces.Shard;
using Drama.Core.Interfaces;
using Orleans;
using Orleans.Providers;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Drama.Auth.Grains.Shard
{
	[StorageProvider(ProviderName = StorageProviders.Infrastructure)]
	public class ShardList : Grain<List<string>>, IShardList
	{
		public Task AddShardKey(string key)
		{
			if (!State.Contains(key))
				State.Add(key);

			return WriteStateAsync();
		}

		public Task AddShardKey(string key, string ip, int port)
		{
			throw new NotImplementedException();
		}

		public Task<IList<string>> GetShardKeys()
			=> Task.FromResult<IList<string>>(State);
	}
}
