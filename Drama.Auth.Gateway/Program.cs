using Drama.Auth.Gateway.Configuration;
using Drama.Auth.Interfaces.Account;
using Drama.Auth.Interfaces.Shard;
using Drama.Core.Gateway.Networking;
using Drama.Core.Interfaces;
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
      var configFile = "AuthGateway.json";
      if (args.Length >= 1)
        configFile = args[0];
      
      var config = GetConfiguration(configFile);

      if (config == null)
      {
        Console.WriteLine($"unable to find {nameof(AuthGatewayConfiguration)} element in json file {configFile}");
        return;
      }

      using (var server = new TcpServer(IPAddress.Parse(config.Server.BindAddress), config.Server.BindPort, config.Server.AcceptQueue, config.Server.ReceiveBufferBlockSize, config.Server.ReceiveBufferPoolSize))
      {
        // since the debugger will start the host and gateway at the same time, it's handy to put a pause here
        if (Debugger.IsAttached)
        {
          Console.WriteLine("running with attached debugger; press enter to start");
          Console.ReadLine();
        }

        Console.Write("starting orleans...");
        GrainClient.Initialize(GetOrleansConfiguration(config));
        Console.WriteLine("done!");

        try
        {
          server.ClientConnected += async (sender, e) =>
          {
            // the packet filter hooks events on the session, which keeps it alive as long as the session lives
            var filter = new AuthPacketRouter(e.Session, GrainClient.GrainFactory);

            // the packet filter must be created before any awaits in this block so it can listen for the logon challenge immediately
            await filter.InitializeAsync();
          };

          Console.Write("starting tcp server...");
          server.Start();

          try
          {
            Console.WriteLine("done!");
            Console.WriteLine("use stop command to stop the gateway");

						do
						{
							Console.Write("auth> ");
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

    private static AuthGatewayConfiguration GetConfiguration(string filename)
    {
      var config = new ConfigurationBuilder()
        .AddJsonFile(filename)
        .Build();

      return config.GetSection(nameof(AuthGatewayConfiguration)).Get<AuthGatewayConfiguration>();
    }

    private static ClientConfiguration GetOrleansConfiguration(AuthGatewayConfiguration config)
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
					case "account.create":
						{
							var account = GrainClient.GrainFactory.GetGrain<IAccount>(split[1].ToUpperInvariant());
							var result = account.Create(split[2], AccountSecurityLevel.Normal).Result;

							Console.WriteLine($"### account {result.Name} created successfully");
						}
						break;
					case "shard.create":
						if (Enum.TryParse<ShardType>(split[4], out var shardType))
						{
							var shard = GrainClient.GrainFactory.GetGrain<IShard>(split[1]);
							var result = shard.Create(split[2], Convert.ToInt32(split[3]), shardType, ShardFlags.Recommended).Result;
							var shardList = GrainClient.GrainFactory.GetGrain<IShardList>(0);
							shardList.AddShardKey(shard.GetPrimaryKeyString()).Wait();

							Console.WriteLine($"### shard {shard.GetPrimaryKeyString()} created and added to shard list");
						}
						else
							Console.WriteLine($"### invalid shard type '{split[4]}'");
						break;
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
