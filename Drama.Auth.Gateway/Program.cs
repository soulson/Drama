using Drama.Auth.Gateway.Configuration;
using Drama.Core.Gateway.Networking;
using Microsoft.Extensions.Configuration;
using Orleans;
using Orleans.Runtime.Configuration;
using System;
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

        Console.WriteLine("press enter to start");
        Console.ReadLine();

        Console.Write("initializing orleans...");
        GrainClient.Initialize(GetOrleansConfiguration(config));
        Console.WriteLine("done!");

        try
        {
          Console.Write("starting server...");
          server.Start();

          try
          {
            Console.WriteLine("done!");
            //var helloGrain = GrainClient.GrainFactory.GetGrain<IHello>(0);
            //Console.WriteLine(helloGrain.SayHello("what's up").Result);

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
      var orleansConfig = new ClientConfiguration();

      foreach (var silo in config.Orleans.Silos)
        orleansConfig.Gateways.Add(new IPEndPoint(IPAddress.Parse(silo.Address), silo.Port));

      return orleansConfig;
    }
  }
}
