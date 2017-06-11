using Drama.Auth.Interfaces;
using Orleans;
using System;
using System.Threading.Tasks;
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

    public Task<RealmListResponse> GetRealmList(RealmListRequest packet)
    {
      GetLogger().Info("got realm list request");
      return Task.FromException<RealmListResponse>(new NotImplementedException());
    }

		public async Task<LogonChallengeResponse> SubmitLogonChallenge(LogonChallengeRequest packet)
		{
			GetLogger().Info($"got logon challenge request from {packet.Identity}");

			var account = GrainFactory.GetGrain<IAccount>(packet.Identity);

			// there is no promise that the account will still exist after this call, but it's nice to check anyways
			if (await account.Exists())
			{
				try
				{
					var initialParams = await account.GetSrpInitialParameters();

					return new LogonChallengeResponse()
					{
						Result = AuthResponseOpcode.Success,
						B = initialParams.B,
						G = initialParams.G,
						N = initialParams.N,
						RandomNumber = initialParams.RandomNumber,
						Salt = initialParams.Salt,
					};
				}
				catch (AccountDoesNotExistException)
				{
					return new LogonChallengeResponse()
					{
						Result = AuthResponseOpcode.FailBadCredentials,
					};
				}
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

    public Task<LogonProofResponse> SubmitLogonProof(LogonProofRequest packet)
    {
      GetLogger().Info("got logon proof request");
      return Task.FromResult(new LogonProofResponse() { Result = AuthResponseOpcode.FailBadCredentials });
    }
  }
}
