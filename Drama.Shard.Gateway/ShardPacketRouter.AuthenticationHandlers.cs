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

using Drama.Shard.Interfaces.Protocol;
using Drama.Shard.Interfaces.Session;
using System;
using System.Threading.Tasks;

namespace Drama.Shard.Gateway
{
	public partial class ShardPacketRouter
	{
		[Handler(typeof(AuthSessionRequest))]
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
