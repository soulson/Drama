using Drama.Core.Interfaces.Networking;
using Drama.Shard.Interfaces.Objects;
using System.IO;
using System.Text;

namespace Drama.Shard.Interfaces.Protocol
{
	[ClientPacket(ShardClientOpcode.PlayerLogin)]
	public sealed class PlayerLoginRequest : IInPacket
	{
		public ObjectID CharacterId { get; set; }

		public bool Read(Stream stream)
		{
			try
			{
				using (var reader = new BinaryReader(stream, Encoding.UTF8, true))
					CharacterId = new ObjectID(reader.ReadInt64());

				return true;
			}
			catch (EndOfStreamException)
			{
				return false;
			}
		}
	}
}
