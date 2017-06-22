using Orleans;
using System;
using System.Threading.Tasks;

namespace Drama.Shard.Interfaces.Session
{
	public interface IShardSession : IGrainWithGuidKey
	{
		Task Connect(IShardSessionObserver observer);
		Task Disconnect(IShardSessionObserver observer);
	}
}
