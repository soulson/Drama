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

using Drama.Core.Gateway.Networking;
using Drama.Core.Interfaces;
using Drama.Shard.Gateway;
using Drama.Shard.Gateway.Configuration;
using Microsoft.Extensions.Configuration;
using Orleans;
using Orleans.Runtime;
using Orleans.Runtime.Configuration;
using System;
using System.Diagnostics;
using System.Net;

namespace Drama.Auth.Gateway
{
	public static class Program
	{
		public static void Main(string[] args)
		{
			var configFile = "ShardGateway.json";
			if (args.Length >= 1)
				configFile = args[0];

			var config = GetConfiguration(configFile);

			if (config == null)
			{
				Console.WriteLine($"unable to find {nameof(ShardGatewayConfiguration)} element in json file {configFile}");
				return;
			}

			using (var server = new TcpServer(IPAddress.Parse(config.Server.BindAddress), config.Server.BindPort, config.Server.AcceptQueue, config.Server.ReceiveBufferBlockSize, config.Server.ReceiveBufferPoolSize))
			{
				// since the debugger will start the host and gateway at the same time, it's handy to put a pause here
				if (Debugger.IsAttached)
				{
					Console.WriteLine("shard gateway running with attached debugger; press enter to start");
					Console.ReadLine();
				}

				Console.Write("starting orleans...");
				GrainClient.Initialize(GetOrleansConfiguration(config));
				Console.WriteLine("done!");

				try
				{
					// start a packet router for each client that connects
					server.ClientConnected += async (sender, e) =>
					{
						// the packet router hooks events on the session, which keeps it alive as long as the session lives
						var router = new ShardPacketRouter(e.Session, GrainClient.GrainFactory, config.ShardName);

						// the packet router must be created before any awaits in this block so it can listen immediately
						await router.InitializeAsync();
					};

					Console.Write("starting tcp server...");
					server.Start();

					try
					{
						Console.WriteLine("done!");
						Console.WriteLine("use stop command to stop the gateway");

						do
						{
							Console.Write("shard> ");
						} while (ProcessCommand(Console.ReadLine()));
					}
					finally
					{
						Console.Write("stopping server...");
						server.Stop();
						Console.WriteLine("done!");
					}
				}
				finally
				{
					Console.Write("stopping orleans...");
					GrainClient.Uninitialize();
					Console.WriteLine("done!");
				}
			}

			// the debugger will also kill the host and gateway as soon as one terminates, so this lets us wait for a clean shutdown
			if (Debugger.IsAttached)
			{
				Console.WriteLine("running with attached debugger; press enter to quit");
				Console.ReadLine();
			}
		}

		private static ShardGatewayConfiguration GetConfiguration(string filename)
		{
			var config = new ConfigurationBuilder()
				.AddJsonFile(filename)
				.Build();

			return config.GetSection(nameof(ShardGatewayConfiguration)).Get<ShardGatewayConfiguration>();
		}

		private static ClientConfiguration GetOrleansConfiguration(ShardGatewayConfiguration config)
		{
			var orleansConfig = new ClientConfiguration()
			{
				DefaultTraceLevel = Severity.Info,
				TraceToConsole = true,
				TraceFilePattern = "none",
			};

			foreach (var silo in config.Orleans.Silos)
				orleansConfig.Gateways.Add(new IPEndPoint(IPAddress.Parse(silo.Address), silo.Port));

			return orleansConfig;
		}

		// returns false if command is to shutdown
		private static bool ProcessCommand(string line)
		{
			string[] split = line?.Split(' ') ?? throw new ArgumentNullException(nameof(line));

			if (split.Length == 0)
				return true;

			try
			{
				switch (split[0])
				{
					case "stop":
						return false;
					default:
						Console.WriteLine($"### unrecognized command '{split[0]}'");
						break;
				}
			}
			catch (DramaException ex)
			{
				Console.WriteLine($"### command failed: {ex.Message}");
			}

			return true;
		}
	}
}
