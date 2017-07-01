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

using Orleans.Runtime;
using Orleans.Runtime.Configuration;
using Orleans.Runtime.Host;
using System;
using System.Diagnostics;
using System.Net;

namespace Drama.Host
{
	public static class Program
	{
		public static void Main(string[] args)
		{
			var configFile = "DramaHost.xml";
			if (args.Length >= 1)
				configFile = args[0];

			var clusterConfig = new ClusterConfiguration();
			clusterConfig.LoadFromFile(configFile);

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
