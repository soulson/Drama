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

using Drama.Core.Interfaces;
using Drama.Shard.Interfaces.Formats.Dbc;
using Drama.Shard.Interfaces.Utilities;
using Orleans;
using System;
using System.IO;
using System.Linq;
using System.Reflection;

namespace Drama.Tools.Load.Formats.Dbc
{
	/// <summary>
	/// Loads the contents of a DBC file into static storage.
	/// </summary>
	/// <typeparam name="TGrain">
	/// Type of the Grain providing static storage
	/// </typeparam>
	/// <typeparam name="TEntity">
	/// Type of the entity used by that Grain to persist state
	/// </typeparam>
	public class DbcLoader<TGrain, TEntity>
		where TGrain : IGrainWithIntegerKey, IMergeable<TEntity>
		where TEntity : new()
	{
		/// <summary>
		/// Gets the path to a folder containing DBC files to read.
		/// </summary>
		protected string DbcPath { get; }

		/// <summary>
		/// Creates a new DbcLoader that will read DBC files from the specified
		/// path.
		/// </summary>
		public DbcLoader(string dbcPath)
		{
			DbcPath = dbcPath ?? throw new ArgumentNullException(nameof(dbcPath));
		}

		/// <summary>
		/// Loads all rows from TEntity's associated DBC file into static storage
		/// provided by TGrain.
		/// </summary>
		/// <param name="grainFactory">The IGrainFactory to load into</param>
		public void LoadEntities(IGrainFactory grainFactory)
		{
			var entityType = typeof(TEntity);
			var fileName = entityType.GetTypeInfo().GetCustomAttribute<DbcEntityAttribute>()?.DbcFileName
				?? throw new DramaException($"type {typeof(TEntity).Name} does not have a {nameof(DbcEntityAttribute)} but is used as a DbcEntity");
			var idProperty = GetIdProperty(entityType);

			using (var dbc = new Dbc<TEntity>(new FileStream(Path.Combine(DbcPath, fileName), FileMode.Open, FileAccess.Read, FileShare.Read)))
			{
				foreach (var row in dbc)
				{
					var idValue = Convert.ToInt64(idProperty.GetValue(row));
					var grain = grainFactory.GetGrain<TGrain>(idValue);

					grain.Clear().Wait();
					grain.Merge(row).Wait();
				}
			}
		}

		private PropertyInfo GetIdProperty(Type type)
		{
			var properties =
				from property in type.GetProperties()
				where property.GetCustomAttribute<DbcKeyAttribute>() != null
					 && property.GetMethod != null
				select property;

			var candidateKeys = properties.ToList();

			if (candidateKeys.Count < 1)
				throw new ArgumentException($"this type does not have any fields with {nameof(DbcKeyAttribute)}", nameof(type));
			if (candidateKeys.Count > 1)
				throw new ArgumentException($"this type has {candidateKeys.Count} fields with {nameof(DbcKeyAttribute)} but expected 1", nameof(type));

			return properties.First();
		}
	}
}
