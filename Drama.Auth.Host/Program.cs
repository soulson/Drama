using Orleans.Runtime.Host;
using System;
using System.IO;
using System.Net;

namespace Drama.Auth.Host
{
  public static class Program
  {
    public static void Main(string[] args)
    {
      using (var siloHost = new SiloHost(Dns.GetHostName(), new FileInfo("AuthHost.Orleans.xml")))
      {
        siloHost.InitializeOrleansSilo();
        if (!siloHost.StartOrleansSilo())
        {
          Console.WriteLine($"failed to start orleans silo {siloHost.Name} as a {siloHost.Type} node");
          return;
        }

        Console.WriteLine("press enter to close");
        Console.ReadLine();
      }
    }
  }
}
