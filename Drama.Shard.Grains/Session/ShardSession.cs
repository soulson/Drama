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
using Drama.Auth.Interfaces.Utilities;
using Drama.Core.Interfaces.Networking;
using Drama.Shard.Interfaces.Characters;
using Drama.Shard.Interfaces.Protocol;
using Drama.Shard.Interfaces.Session;
using Orleans;
using Orleans.Runtime;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;

namespace Drama.Shard.Grains.Session
{
	public partial class ShardSession : Grain, IShardSession
	{
		private readonly ObserverSubscriptionManager<IShardSessionObserver> sessionObservers;

		private int seed;

		private string AuthenticatedIdentity { get; set; }
		private string ShardName { get; set; }
		private ICharacter ActiveCharacter { get; set; }

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

		protected void VerifyAuthenticated([CallerMemberName] string callerName = "<noname>")
		{
			if (AuthenticatedIdentity == null)
				throw new SessionStateException($"{callerName} can only be called while authenticated");
		}

		protected void VerifyIngame([CallerMemberName] string callerName = "<noname>")
		{
			VerifyAuthenticated(callerName);

			if (ActiveCharacter == null)
				throw new SessionStateException($"{callerName} can only be called while ingame");
		}
	}
}
