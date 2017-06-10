using Drama.Core.Gateway.Networking;
using Orleans;
using System;
using System.Net;

namespace Drama.Auth.Gateway
{
  public class Program
  {
    public static void Main(string[] args)
    {
      using (var server = new TcpServer(IPAddress.Parse("127.0.0.1"), 3724, 64, 2048, 128))
      {
        server.ClientConnected += (sender, e) => Console.WriteLine($"client {e.Session.Id} connected");
        server.ClientDisconnected += (sender, e) => Console.WriteLine($"client {e.Session.Id} disconnected");
        server.DataReceived += (sender, e) => Console.WriteLine($"received {e.ReceivedData.Count} bytes from client {e.Session.Id}");
        server.DataSent += (sender, e) => Console.WriteLine($"sent {e.SentData.Count} bytes to client {e.Session.Id}");

        Console.WriteLine("press enter to start");
        Console.ReadLine();

        Console.Write("initializing orleans...");
        //var orleansConfig = ClientConfiguration.LocalhostSilo(30000);
        //GrainClient.Initialize(orleansConfig);
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
          //GrainClient.Uninitialize();
          Console.WriteLine("done!");
        }
      }
    }
  }
}