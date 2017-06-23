using Drama.Auth.Interfaces;
using Drama.Auth.Interfaces.Account;
using Drama.Auth.Interfaces.Utilities;
using Drama.Core.Interfaces.Networking;
using Drama.Core.Interfaces.Security;
using Drama.Core.Interfaces.Utilities;
using Drama.Shard.Interfaces.Protocol;
using Drama.Shard.Interfaces.Session;
using Orleans;
using Orleans.Runtime;
using System;
using System.Numerics;
using System.Security.Cryptography;
using System.Text;
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

		public async Task<BigInteger> Authenticate(AuthSessionRequest authChallenge)
		{
			if (String.IsNullOrEmpty(authChallenge.Identity))
				throw new ArgumentNullException(nameof(authChallenge.Identity));

			if (seed == 0)
			{
				await Send(new AuthSessionResponse() { Response = AuthResponse.Failed });
				throw new AuthenticationFailedException("cannot authenticate with a server seed of 0");
			}

			var account = GrainFactory.GetGrain<IAccount>(authChallenge.Identity);

			if (await account.Exists())
			{
				try
				{
					var sessionKey = await account.GetSessionKey();

					using (var sha1 = new Digester(SHA1.Create()))
					{
						var serverDigest = BigIntegers.FromUnsignedByteArray(
							sha1.CalculateDigest(new byte[][]
							{
								Encoding.UTF8.GetBytes(authChallenge.Identity),
								new byte[4],
								BitConverter.GetBytes(authChallenge.ClientSeed),
								BitConverter.GetBytes(seed),
								sessionKey.ToByteArray(40),
							})
						);

						if (serverDigest == authChallenge.ClientDigest)
						{
							GetLogger().Info($"{authChallenge.Identity} successfully authenticated to {nameof(ShardSession)} {this.GetPrimaryKey()}");
							await Send(new AuthSessionResponse() { Response = AuthResponse.Success });
							return sessionKey;
						}
						else
						{
							await Send(new AuthSessionResponse() { Response = AuthResponse.Failed });
							throw new AuthenticationFailedException($"account {authChallenge.Identity} failed authentication proof");
						}
					}
				}
				catch (AccountDoesNotExistException)
				{
					await Send(new AuthSessionResponse() { Response = AuthResponse.UnknownAccount });
					throw new AuthenticationFailedException($"account {authChallenge.Identity} does not exist");
				}
				catch (AccountStateException)
				{
					GetLogger().Warn($"received {nameof(AuthSessionRequest)} with unauthenticated identity {authChallenge.Identity}");
					await Send(new AuthSessionResponse() { Response = AuthResponse.Failed });
					throw new AuthenticationFailedException($"account {authChallenge.Identity} is not authenticated");
				}
			}
			else
			{
				await Send(new AuthSessionResponse() { Response = AuthResponse.UnknownAccount });
				throw new AuthenticationFailedException($"account {authChallenge.Identity} does not exist");
			}
		}
	}
}
