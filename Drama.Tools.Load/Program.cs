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

using Drama.Shard.Interfaces.Characters;
using Drama.Shard.Interfaces.Creatures;
using Drama.Shard.Interfaces.Maps;
using Drama.Shard.Interfaces.Units;
using Drama.Tools.Load.Configuration;
using Drama.Tools.Load.Formats.Dbc;
using Drama.Tools.Load.Formats.Sql;
using Drama.Tools.Load.Formats.Sql.Entities;
using Microsoft.Extensions.Configuration;
using Orleans;
using Orleans.Runtime;
using Orleans.Runtime.Configuration;
using System;
using System.Diagnostics;
using System.Net;

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

			Console.Write("loading configuration...");
			var config = GetConfiguration(configFileName);
			Console.WriteLine("done!");

			Console.Write("starting orleans...");
			GrainClient.Initialize(GetOrleansConfiguration(config));
			Console.WriteLine("done!");

			try
			{
				Console.WriteLine($"loading dbc {nameof(MapDefinitionEntity)}");
				new DbcLoader<IMapDefinition, MapDefinitionEntity>(config.Dbc.Path).LoadEntities(GrainClient.GrainFactory);
				Console.WriteLine($"loading dbc {nameof(RaceDefinitionEntity)}");
				new DbcLoader<IRaceDefinition, RaceDefinitionEntity>(config.Dbc.Path).LoadEntities(GrainClient.GrainFactory);

				using (var context = new ElysiumContext(config.Sql.Address, config.Sql.Port, config.Sql.Schema, config.Sql.User, config.Sql.Password))
				{
					Console.WriteLine($"loading sql {nameof(CharacterTemplateEntity)}");
					new SqlLoader<ICharacterTemplate, CharacterTemplateEntity, PlayerCreateInfo>(context).LoadEntities(GrainClient.GrainFactory);
					Console.WriteLine($"loading sql {nameof(CreatureDefinitionEntity)}");
					new SqlLoader<ICreatureDefinition, CreatureDefinitionEntity, CreatureTemplate>(context).LoadEntities(GrainClient.GrainFactory);
					Console.WriteLine($"loading sql set {nameof(CreatureSpawnPoint)}");
					new SqlSetLoader<ICreatureSpawnSet, CreatureSpawnPoint, Creature>(context).LoadEntities(GrainClient.GrainFactory);
				}
			}
			finally
			{
				Console.Write("stopping orleans...");
				GrainClient.Uninitialize();
				Console.WriteLine("done!");
			}

			if (Debugger.IsAttached)
			{
				Console.WriteLine("running with attached debugger; press enter to quit");
				Console.ReadLine();
			}
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