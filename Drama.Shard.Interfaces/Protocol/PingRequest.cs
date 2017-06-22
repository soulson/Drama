using Drama.Core.Interfaces.Networking;
using System;
using System.IO;
using System.Text;

namespace Drama.Shard.Interfaces.Protocol
{
	[ClientPacket(ShardClientOpcode.Ping)]
	public sealed class PingRequest : IInPacket
	{
		public int Cookie { get; set; }
		public int Latency { get; set; }

		public bool Read(Stream stream)
		{
			try
			{
				using (var reader = new BinaryReader(stream, Encoding.UTF8, true))
				{
					Cookie = reader.ReadInt32();
					Latency = reader.ReadInt32();
				}

				return true;
			}
			catch (EndOfStreamException)
			{
				return false;
			}
		}
	}
}
