using Drama.Core.Interfaces.Networking;
using Orleans;
using System;

namespace Drama.Shard.Interfaces.Session
{
	public interface IShardSessionObserver : IGrainObserver
	{
		void ForwardPacket(IOutPacket packet);
	}
}
