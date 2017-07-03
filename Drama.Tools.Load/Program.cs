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
using Drama.Tools.Load.Formats.Dbc;
using System;
using System.IO;
using System.Reflection;

namespace Drama.Tools.Load
{
	public class Program
	{
		public static void Main(string[] args)
		{
			string dbcPath;
			if (args.Length < 1)
			{
				Console.Write("dbc path: ");
				dbcPath = Console.ReadLine();
			}
			else
				dbcPath = args[0];

			var fileName = typeof(MapDefinitionEntity).GetTypeInfo().GetCustomAttribute<DbcEntityAttribute>().DbcFileName;
			using (var dbc = new Dbc<MapDefinitionEntity>(new FileStream(Path.Combine(dbcPath, fileName), FileMode.Open, FileAccess.Read, FileShare.Read)))
			{
				foreach(var row in dbc)
				{
					Console.WriteLine($"{row.Id} {row.Name} {row.Type} {row.MaxPlayerCount} {row.MinLevel} {row.MaxLevel}");
				}
			}

			Console.ReadLine();
		}
	}
}