using Drama.Auth.Interfaces;
using Drama.Auth.Interfaces.Utilities;
using Drama.Core.Interfaces.Networking;
using Drama.Shard.Interfaces.Protocol;
using Drama.Shard.Interfaces.Session;
using Orleans;
using Orleans.Runtime;
using System.Threading.Tasks;

namespace Drama.Shard.Grains.Session
{
	public partial class ShardSession : Grain, IShardSession
	{
		private readonly ObserverSubscriptionManager<IShardSessionObserver> sessionObservers;

		private int seed;

		private string AuthenticatedIdentity { get; set; }
		private string ShardName { get; set; }

		public ShardSession()
		{
			sessionObservers = new ObserverSubscriptionManager<IShardSessionObserver>();
		}

		public async Task Connect(IShardSessionObserver observer, string shardName)
		{
			if (sessionObservers.Count != 0)
				GetLogger().Warn($"session {this.GetPrimaryKey()} has {sessionObservers.Count} observers at the start of {nameof(Connect)}; expected 0");

			ShardName = shardName;
			sessionObservers.Subscribe(observer);
			GetLogger().Info($"session {this.GetPrimaryKey()} connected");

			var random = GrainFactory.GetGrain<IRandomService>(0);

			do
			{
				seed = await random.GetRandomInt();
			} while (seed == 0);

			var randomNumber = await random.GetRandomBigInteger(32);

			var authChallenge = new AuthChallengeRequest()
			{
				Seed = seed,
				RandomNumber = randomNumber,
			};

			await Send(authChallenge);
		}

		public Task Disconnect(IShardSessionObserver observer)
		{
			sessionObservers.Unsubscribe(observer);
			seed = 0;
			GetLogger().Info($"session {this.GetPrimaryKey()} disconnected");
			return Task.CompletedTask;
		}

		public Task Send(IOutPacket packet)
		{
			sessionObservers.Notify(receiver => receiver.ForwardPacket(packet));
			return Task.CompletedTask;
		}
	}
}
