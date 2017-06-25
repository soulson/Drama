using Drama.Core.Interfaces.Networking;
using Orleans.Concurrency;
using System.IO;

namespace Drama.Shard.Interfaces.Protocol
{
	[Immutable]
	[ClientPacket(ShardClientOpcode.CharacterList)]
	public sealed class CharacterListRequest : IInPacket
	{
		public bool Read(Stream stream)
			=> true;
	}
}
