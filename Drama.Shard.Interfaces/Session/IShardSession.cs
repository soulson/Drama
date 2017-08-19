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

using Drama.Core.Interfaces.Networking;
using Drama.Shard.Interfaces.Characters;
using Drama.Shard.Interfaces.Objects;
using Drama.Shard.Interfaces.Protocol;
using Orleans;
using Orleans.Concurrency;
using System.Collections.Generic;
using System.Numerics;
using System.Threading.Tasks;

namespace Drama.Shard.Interfaces.Session
{
	/// <summary>
	/// ShardSession grains represent a stream of communication between a client
	/// and a shard.
	/// 
	/// The key for this grain is the Guid of the TcpSession that initiated it.
	/// </summary>
	public interface IShardSession : IGrainWithGuidKey
	{
		/// <summary>
		/// Connects a network-layer session to a new ShardSession.
		/// </summary>
		/// <param name="observer">
		/// An object to receive notifications that this ShardSession needs to send
		/// to the session's client.
		/// 
		/// Cannot be null
		/// </param>
		/// <param name="shardName">
		/// Name of the shard that this ShardSession is connecting to.
		/// 
		/// Cannot be null
		/// </param>
		Task Connect(IShardSessionObserver observer, string shardName);

		/// <summary>
		/// Disconnects a network-layer session from the ShardSession.
		/// </summary>
		/// <param name="observer">
		/// This must be the same object that was passed to the Connect method.
		/// 
		/// Cannot be null
		/// </param>
		Task Disconnect(IShardSessionObserver observer);

		/// <summary>
		/// Forwards a notification to the network-layer session associated with
		/// this ShardSession. The network layer is responsible for forwarding it
		/// to the session client.
		/// 
		/// This method is one-way.
		/// </summary>
		/// <param name="packet">Cannot be null</param>
		[OneWay]
		Task Send(IOutPacket packet);

		#region Authentication
		/// <summary>
		/// Authenticates a client with the ShardSession.
		/// </summary>
		/// <param name="authChallenge">Cannot be null</param>
		/// <returns>Session key for the ShardSession</returns>
		Task<BigInteger> Authenticate(AuthSessionRequest authChallenge);

		/// <summary>
		/// Informs the client of the result of session authentication.
		/// </summary>
		/// <param name="authRequest">Cannot be null</param>
		Task Handshake(AuthSessionRequest authRequest);
		#endregion

		#region Characters
		/// <summary>
		/// Gets a list of Character entities owned by the autenticated account on
		/// this session's shard.
		/// </summary>
		Task<IList<CharacterEntity>> GetCharacterList();

		/// <summary>
		/// Creates a new Character owned by the authenticated account on this
		/// session's shard.
		/// </summary>
		/// <param name="request">Cannot be null</param>
		Task CreateCharacter(CharacterCreateRequest request);

		/// <summary>
		/// Logs this session into the world with the Character represented by
		/// characterId.
		/// </summary>
		Task Login(ObjectID characterId);
		#endregion

		#region Movement
		/// <summary>
		/// Sets the movement mode of the ingame player's controlled unit.
		/// </summary>
		Task Move(MovementInPacket request);
		#endregion

		#region Queries
		/// <summary>
		/// Looks up the name, race, class, and sex of a Character.
		/// </summary>
		Task<QueryNameResponse> QueryName(QueryNameRequest request);
		#endregion
	}
}
