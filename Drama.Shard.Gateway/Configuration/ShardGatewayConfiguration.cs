using System;
using Drama.Core.Gateway.Configuration;

namespace Drama.Shard.Gateway.Configuration
{
	public class ShardGatewayConfiguration
	{
		public TcpServerConfiguration Server { get; set; }
		public OrleansClientConfiguration Orleans { get; set; }
	}
}
