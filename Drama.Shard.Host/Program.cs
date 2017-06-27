using Drama.Core.Interfaces;
using Drama.Shard.Host.Providers;
using Orleans.Runtime;
using Orleans.Runtime.Configuration;
using Orleans.Runtime.Host;
using Orleans.Storage;
using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Diagnostics;
using System.Net;

namespace Drama.Shard.Host
{
	public static class Program
	{
		public static void Main(string[] args)
		{
			var clusterConfig = new ClusterConfiguration();
			clusterConfig.LoadFromFile("ShardHost.Orleans.xml");

			var siloConfig = clusterConfig.Defaults;
			siloConfig.DefaultTraceLevel = Severity.Info;
			siloConfig.TraceToConsole = true;
			siloConfig.TraceFilePattern = "none";

			//clusterConfig.Globals.RegisterStorageProvider<MemoryStorage>(StorageProviders.Account);
			//clusterConfig.Globals.RegisterStorageProvider<MemoryStorage>(StorageProviders.DynamicWorld);
			//clusterConfig.Globals.RegisterStorageProvider<MemoryStorage>(StorageProviders.StaticWorld);
			//clusterConfig.Globals.RegisterStorageProvider<MemoryStorage>(StorageProviders.Infrastructure);
			clusterConfig.Globals.RegisterStorageProvider<FileStorage>(StorageProviders.Infrastructure, ImmutableDictionary.CreateRange(new[]
			{
				new KeyValuePair<string, string>("RootDirectory", @"C:\Users\foxic\Desktop\dramastore\infrastructure"),
				new KeyValuePair<string, string>("IndentJSON", "true"),
			}));
			clusterConfig.Globals.RegisterStorageProvider<FileStorage>(StorageProviders.Account, ImmutableDictionary.CreateRange(new[]
			{
				new KeyValuePair<string, string>("RootDirectory", @"C:\Users\foxic\Desktop\dramastore\account"),
				new KeyValuePair<string, string>("IndentJSON", "true"),
			}));
			clusterConfig.Globals.RegisterStorageProvider<FileStorage>(StorageProviders.StaticWorld, ImmutableDictionary.CreateRange(new[]
			{
				new KeyValuePair<string, string>("RootDirectory", @"C:\Users\foxic\Desktop\dramastore\staticworld"),
				new KeyValuePair<string, string>("IndentJSON", "true"),
			}));
			clusterConfig.Globals.RegisterStorageProvider<FileStorage>(StorageProviders.DynamicWorld, ImmutableDictionary.CreateRange(new[]
			{
				new KeyValuePair<string, string>("RootDirectory", @"C:\Users\foxic\Desktop\dramastore\dynamicworld"),
				new KeyValuePair<string, string>("IndentJSON", "true"),
			}));

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
