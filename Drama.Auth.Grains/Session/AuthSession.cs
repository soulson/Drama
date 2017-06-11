using Drama.Auth.Interfaces;
using Orleans;
using System;
using System.Threading.Tasks;
using Drama.Auth.Interfaces.Packets;
using Drama.Auth.Interfaces.Session;
using Orleans.Runtime;
using Drama.Auth.Interfaces.Account;
using Drama.Auth.Interfaces.Protocol;

namespace Drama.Auth.Grains.Session
{
	// TODO: remove observer stuff once used as a reference implementation for ShardSession
  public class AuthSession : Grain, IAuthSession
  {
		private readonly ObserverSubscriptionManager<IAuthSessionObserver> sessionObservers;

    public AuthSession()
    {
      sessionObservers = new ObserverSubscriptionManager<IAuthSessionObserver>();
    }

		public Task Connect(IAuthSessionObserver observer)
    {
      sessionObservers.Subscribe(observer);
      GetLogger().Info($"{this.GetPrimaryKey()} connected");
      return Task.CompletedTask;
    }

    public Task Disconnect(IAuthSessionObserver observer)
    {
      sessionObservers.Unsubscribe(observer);
      GetLogger().Info($"{this.GetPrimaryKey()} disconnected");
      return Task.CompletedTask;
    }

    public Task GetRealmList(RealmListRequest packet)
    {
      GetLogger().Info("got realm list request");
      return Task.CompletedTask;
    }

		public async Task<LogonChallengeResponse> SubmitLogonChallenge(LogonChallengeRequest packet)
		{
			GetLogger().Info($"got logon challenge request from {packet.Identity}");

			var account = GrainFactory.GetGrain<IAccount>(packet.Identity);

			if (await account.Exists())
			{
				// obvious todo
				return new LogonChallengeResponse()
				{
					Result = AuthResponseOpcode.FailBusy,
				};
			}
			else
			{
				// account does not exist
				return new LogonChallengeResponse()
				{
					Result = AuthResponseOpcode.FailBadCredentials,
				};
			}
		}

    public Task SubmitLogonProof(LogonProofRequest packet)
    {
      GetLogger().Info("got logon proof request");
      return Task.CompletedTask;
    }
  }
}
