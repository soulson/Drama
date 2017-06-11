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

		// just because the account is associated does NOT mean it is authenticated!
		private IAccount AssociatedAccount { get; set; }

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
			return Task.FromResult(new RealmListResponse());
    }

		public async Task<LogonChallengeResponse> SubmitLogonChallenge(LogonChallengeRequest packet)
		{
			GetLogger().Info($"got logon challenge request from {packet.Identity}");

			AssociatedAccount = GrainFactory.GetGrain<IAccount>(packet.Identity);

			// there is no promise that the account will still exist after this call, but it's nice to check anyways
			if (await AssociatedAccount.Exists())
			{
				try
				{
					var initialParams = await AssociatedAccount.GetSrpInitialParameters();

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

    public async Task<LogonProofResponse> SubmitLogonProof(LogonProofRequest packet)
    {
      GetLogger().Info("got logon proof request");
			var result = await AssociatedAccount.SrpHandshake(packet.A, packet.M1);

			if(result.Match)
			{
				return new LogonProofResponse()
				{
					Result = AuthResponseOpcode.Success,
					M2 = result.M2,
				};
			}
			else
			{
				return new LogonProofResponse()
				{
					Result = AuthResponseOpcode.FailBadCredentials,
				};
			}
		}
  }
}
