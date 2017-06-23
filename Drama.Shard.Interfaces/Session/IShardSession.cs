using Drama.Core.Interfaces.Networking;
using Drama.Shard.Interfaces.Protocol;
using Orleans;
using System;
using System.Numerics;
using System.Threading.Tasks;

namespace Drama.Shard.Interfaces.Session
{
	public interface IShardSession : IGrainWithGuidKey
	{
		Task Connect(IShardSessionObserver observer);
		Task Disconnect(IShardSessionObserver observer);
		Task Send(IOutPacket packet);

		Task<BigInteger> Authenticate(AuthSessionRequest authChallenge);
		Task Handshake(AuthSessionRequest authRequest);
	}
}
