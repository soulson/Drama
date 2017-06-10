using Drama.Auth.Gateway.Configuration;
using Drama.Core.Gateway.Networking;
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
        server.ClientConnected += (sender, e) => Console.WriteLine($"client {e.Session.Id} connected");
        server.ClientDisconnected += (sender, e) => Console.WriteLine($"client {e.Session.Id} disconnected");
        server.DataReceived += (sender, e) => Console.WriteLine($"received {e.ReceivedData.Count} bytes from client {e.Session.Id}");
        server.DataSent += (sender, e) => Console.WriteLine($"sent {e.SentData.Count} bytes to client {e.Session.Id}");

        // since the debugger will start the silo and client at the same time, it's handy to put a pause here
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
            // the packet serializer hooks events on the session, which keeps it alive as long as the session lives
            var serializer = new PacketSerializer(e.Session, GrainClient.GrainFactory);
            await serializer.InitializeAsync();
          };

          Console.Write("starting tcp server...");
          server.Start();

          try
          {
            Console.WriteLine("done!");

            Console.WriteLine("press enter to stop listening");
            Console.ReadLine();
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
  }
}
