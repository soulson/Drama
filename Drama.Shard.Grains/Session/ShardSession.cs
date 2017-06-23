using Drama.Auth.Interfaces;
using Drama.Auth.Interfaces.Utilities;
using Drama.Shard.Interfaces.Protocol;
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

		private int seed;

		public ShardSession()
		{
			sessionObservers = new ObserverSubscriptionManager<IShardSessionObserver>();
		}

		public async Task Connect(IShardSessionObserver observer)
		{
			if (sessionObservers.Count != 0)
				GetLogger().Warn($"session {this.GetPrimaryKey()} has {sessionObservers.Count} observers at the start of {nameof(Connect)}; expected 0");

			sessionObservers.Subscribe(observer);
			GetLogger().Info($"session {this.GetPrimaryKey()} connected");

			var random = GrainFactory.GetGrain<IRandomService>(0);

			seed = await random.GetRandomInt();
			var randomNumber = await random.GetRandomBigInteger(32);

			var authChallenge = new AuthChallengeRequest()
			{
				Seed = seed,
				RandomNumber = randomNumber,
			};

			sessionObservers.Notify(receiver => receiver.ReceivePacket(authChallenge));
		}

		public Task Disconnect(IShardSessionObserver observer)
		{
			sessionObservers.Unsubscribe(observer);
			seed = 0;
			GetLogger().Info($"session {this.GetPrimaryKey()} disconnected");
			return Task.CompletedTask;
		}
	}
}
