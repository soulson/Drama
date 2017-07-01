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
using System.Collections.Generic;
using System.Numerics;
using System.Threading.Tasks;

namespace Drama.Shard.Interfaces.Session
{
	public interface IShardSession : IGrainWithGuidKey
	{
		Task Connect(IShardSessionObserver observer, string shardName);
		Task Disconnect(IShardSessionObserver observer);
		Task Send(IOutPacket packet);

		#region Authentication
		Task<BigInteger> Authenticate(AuthSessionRequest authChallenge);
		Task Handshake(AuthSessionRequest authRequest);
		#endregion

		#region Characters
		Task<IList<CharacterEntity>> GetCharacterList();
		Task CreateCharacter(CharacterCreateRequest request);
		Task Login(ObjectID characterId);
		#endregion
	}
}
