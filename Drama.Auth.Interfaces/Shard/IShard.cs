using Orleans;
using System;
using System.Threading.Tasks;

namespace Drama.Auth.Interfaces.Shard
{
	public interface IShard : IGrainWithStringKey
	{
		Task<bool> Exists();
		Task<ShardEntity> GetEntity();
		Task<ShardEntity> Create(string address, int port, ShardType type, ShardFlags flags);
	}
}
