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

using Drama.Shard.Interfaces.Formats.Dbc;
using Drama.Shard.Interfaces.Maps;
using Drama.Tools.Load.Configuration;
using Drama.Tools.Load.Formats.Dbc;
using Microsoft.Extensions.Configuration;
using Orleans.Runtime;
using Orleans.Runtime.Configuration;
using System;
using System.IO;
using System.Net;
using System.Reflection;

namespace Drama.Tools.Load
{
	public class Program
	{
		public static void Main(string[] args)
		{
			string configFileName;
			if (args.Length >= 1)
				configFileName = args[0];
			else
				configFileName = "Load.json";

			var config = GetConfiguration(configFileName);

			var fileName = typeof(MapDefinitionEntity).GetTypeInfo().GetCustomAttribute<DbcEntityAttribute>().DbcFileName;
			using (var dbc = new Dbc<MapDefinitionEntity>(new FileStream(Path.Combine(config.Dbc.Path, fileName), FileMode.Open, FileAccess.Read, FileShare.Read)))
			{
				foreach(var row in dbc)
				{
					Console.WriteLine($"{row.Id} {row.Name} {row.Type} {row.MaxPlayerCount} {row.MinLevel} {row.MaxLevel}");
				}
			}

			Console.ReadLine();
		}

		private static LoaderConfiguration GetConfiguration(string filename)
		{
			var config = new ConfigurationBuilder()
				.AddJsonFile(filename)
				.Build();

			return config.GetSection(nameof(LoaderConfiguration)).Get<LoaderConfiguration>();
		}

		private static ClientConfiguration GetOrleansConfiguration(LoaderConfiguration config)
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