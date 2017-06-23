using Drama.Shard.Interfaces.Protocol;
using Drama.Shard.Interfaces.Session;
using System;
using System.Threading.Tasks;

namespace Drama.Shard.Gateway
{
	public partial class ShardPacketRouter
	{
		private async Task HandleAuthSession(AuthSessionRequest request)
		{
			try
			{
				// this handler is a little whacky. it can't just send its own response to the client, because the client
				//  expects the response to be encrypted with the session key. so we have a bit of call-and-response here
				//  to manage this
				var sessionKey = await ShardSession.Authenticate(request);
				packetCipher.Initialize(sessionKey);
				await ShardSession.Handshake(request);
			}
			catch (SessionException ex)
			{
				Console.WriteLine(ex.Message);
				authenticationFailed = true;
			}
		}
	}
}
