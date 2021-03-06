﻿/* 
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

			// TODO: logging
			Console.WriteLine($"sent {ShardServerOpcode.Pong} with latency = {request.Latency} and cookie = 0x{request.Cookie:x8}");

			return Task.CompletedTask;
		}
	}
}
