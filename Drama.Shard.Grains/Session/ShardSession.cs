using Drama.Shard.Interfaces.Session;
using Orleans;
using Orleans.Runtime;
using System;
using System.Threading.Tasks;

namespace Drama.Shard.Grains.Session
{
	public class ShardSession : Grain, IShardSession
	{
		private readonly ObserverSubscriptionManager<IShardSessionObserver> sessionObservers;

		public ShardSession()
		{
			sessionObservers = new ObserverSubscriptionManager<IShardSessionObserver>();
		}

		public Task Connect(IShardSessionObserver observer)
		{
			sessionObservers.Subscribe(observer);
			GetLogger().Info($"session {this.GetPrimaryKey()} connected");
			return Task.CompletedTask;
		}

		public Task Disconnect(IShardSessionObserver observer)
		{
			sessionObservers.Unsubscribe(observer);
			GetLogger().Info($"session {this.GetPrimaryKey()} disconnected");
			return Task.CompletedTask;
		}
	}
}
