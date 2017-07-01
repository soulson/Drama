/* 
 * The Drama project: what you get when a bunch of actors try to host a game.
 * Copyright (C) 2017 Soulson
 *
 * This program is free software: you can redistribute it and/or modify
 * it under the terms of the GNU Affero General Public License as
 * published by the Free Software Foundation, either version 3 of the
 * License, or (at your option) any later version.
 *
 * This program is distributed in the hope that it will be useful,
 * but WITHOUT ANY WARRANTY; without even the implied warranty of
 * MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
 * GNU Affero General Public License for more details.
 *
 * You should have received a copy of the GNU Affero General Public License
 * along with this program.  If not, see <http://www.gnu.org/licenses/>.
 */

using Drama.Auth.Interfaces;
using Drama.Auth.Interfaces.Account;
using Drama.Auth.Interfaces.Protocol;
using Drama.Auth.Interfaces.Session;
using Drama.Auth.Interfaces.Shard;
using Orleans;
using Orleans.Runtime;
using System.Threading.Tasks;

namespace Drama.Auth.Grains.Session
{
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
						Result = AuthResponse.Success,
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
						Result = AuthResponse.FailBadCredentials,
					};
				}
			}
			else
			{
				// account does not exist
				return new LogonChallengeResponse()
				{
					Result = AuthResponse.FailBadCredentials,
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
						Result = AuthResponse.Success,
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
						Result = AuthResponse.FailBadCredentials,
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
					Result = AuthResponse.FailBusy,
				};
			}
		}
  }
}
