using Drama.Core.Interfaces.Networking;
using Drama.Shard.Interfaces.Characters;
using Drama.Shard.Interfaces.Protocol;
using Orleans;
using System;
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
		#endregion
	}
}
