using Drama.Auth.Interfaces;
using Orleans;
using System;
using System.Threading.Tasks;
using Drama.Auth.Interfaces.Session;
using Orleans.Runtime;
using Drama.Auth.Interfaces.Account;
using Drama.Auth.Interfaces.Protocol;
using Drama.Auth.Interfaces.Shard;

namespace Drama.Auth.Grains.Session
{
	// TODO: remove observer stuff once used as a reference implementation for ShardSession
  public class AuthSession : Grain, IAuthSession
  {
		private string AuthenticatingIdentity { get; set; }
		private IAccount AuthenticatedAccount { get; set; }

		public Task Connect()
    {
      GetLogger().Info($"session {this.GetPrimaryKey()} connected");
      return Task.CompletedTask;
    }

    public async Task Disconnect()
    {
			if (AuthenticatedAccount == null)
				GetLogger().Info($"session {this.GetPrimaryKey()} (unauthenticated) disconnected");
			else
			{
				await AuthenticatedAccount.Deauthenticate();
				GetLogger().Info($"session {this.GetPrimaryKey()} (authenticated as {AuthenticatingIdentity}) disconnected");
				AuthenticatedAccount = null;
			}

			AuthenticatingIdentity = null;
    }

    public async Task<RealmListResponse> GetRealmList(RealmListRequest packet)
    {
			var response = new RealmListResponse();
			var shardList = GrainFactory.GetGrain<IShardList>(0);

			foreach (var shardKey in await shardList.GetShardKeys())
			{
				var shard = GrainFactory.GetGrain<IShard>(shardKey);

				try
				{
					var shardEntity = await shard.GetEntity();
					response.ShardList.Add(shardEntity);
				}
				catch (ShardDoesNotExistException)
				{
					GetLogger().Warn($"shard key {shardKey} was returned by {nameof(IShardList)} but does not exist");
				}
			}

			return response;
    }

		public async Task<LogonChallengeResponse> SubmitLogonChallenge(LogonChallengeRequest packet)
		{
			var account = GrainFactory.GetGrain<IAccount>(packet.Identity);
			AuthenticatingIdentity = packet.Identity;

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

    public async Task<LogonProofResponse> SubmitLogonProof(LogonProofRequest packet)
    {
			if (AuthenticatingIdentity != null)
			{
				var account = GrainFactory.GetGrain<IAccount>(AuthenticatingIdentity);
				var result = await account.SrpHandshake(packet.A, packet.M1);

				if (result.Match)
				{
					GetLogger().Info($"account {AuthenticatingIdentity} has authenticated successfully");
					AuthenticatedAccount = account;

					return new LogonProofResponse()
					{
						Result = AuthResponseOpcode.Success,
						M2 = result.M2,
					};
				}
				else
				{
					GetLogger().Info($"account {AuthenticatingIdentity} has failed to authenticate");
					await account.Deauthenticate();
					AuthenticatingIdentity = null;

					return new LogonProofResponse()
					{
						Result = AuthResponseOpcode.FailBadCredentials,
					};
				}
			}
			else
			{
				// this most likely means someone is using a modified client and trying to short-circuit SRP in some way.
				//  or there's a bug in the server
				GetLogger().Warn("got logon proof request with no authenticating identity - modified client?");

				return new LogonProofResponse()
				{
					Result = AuthResponseOpcode.FailBusy,
				};
			}
		}
  }
}
