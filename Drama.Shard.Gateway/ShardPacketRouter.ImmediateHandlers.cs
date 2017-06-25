using Drama.Shard.Interfaces.Protocol;
using System;
using System.Threading.Tasks;

namespace Drama.Shard.Gateway
{
	public partial class ShardPacketRouter
	{
		[Handler(typeof(PingRequest))]
		private Task HandlePing(PingRequest request)
		{
			ForwardPacket(new PongResponse() { Cookie = request.Cookie });
			Console.WriteLine($"sent {ShardServerOpcode.Pong} with latency = {request.Latency} and cookie = 0x{request.Cookie:x8}");

			return Task.CompletedTask;
		}
	}
}
