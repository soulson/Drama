using Orleans.Runtime;
using Orleans.Runtime.Configuration;
using Orleans.Runtime.Host;
using System;
using System.Diagnostics;
using System.Net;

namespace Drama.Auth.Host
{
  public static class Program
  {
    public static void Main(string[] args)
    {
      var clusterConfig = new ClusterConfiguration();
      clusterConfig.LoadFromFile("AuthHost.Orleans.xml");

      var siloConfig = clusterConfig.Defaults;
      siloConfig.DefaultTraceLevel = Severity.Info;
      siloConfig.TraceToConsole = true;
      siloConfig.TraceFilePattern = "none";

      using (var siloHost = new SiloHost(Dns.GetHostName(), clusterConfig))
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
      
      // the debugger will kill the host and gateway as soon as either one terminates, so this lets us wait for a clean shutdown
      if (Debugger.IsAttached)
      {
        Console.WriteLine("running with attached debugger; press enter to quit");
        Console.ReadLine();
      }
    }
  }
}
