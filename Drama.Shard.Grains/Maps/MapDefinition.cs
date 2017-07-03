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
using Drama.Shard.Interfaces.Maps;
using Orleans;
using Orleans.Providers;
using System;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

namespace Drama.Shard.Grains.Maps
{
	[StorageProvider(ProviderName = StorageProviders.StaticWorld)]
	public class MapDefinition : Grain<MapDefinitionEntity>, IMapDefinition
	{
		public Task<bool> Exists()
			=> Task.FromResult(State.Exists);

		public Task<MapDefinitionEntity> GetEntity()
		{
			if (!Exists().Result)
				throw new MapDoesNotExistException($"map id {this.GetPrimaryKeyLong()} does not exist");

			return Task.FromResult(State);
		}

		public Task Merge(MapDefinitionEntity input)
		{
			var properties =
				from property in State.GetType().GetProperties()
				where property.SetMethod != null
					 && property.GetMethod != null
				select property;

			foreach(var property in properties)
			{
				if (property.GetValue(input) != GetDefaultValue(property.PropertyType))
					property.SetValue(State, property.GetValue(input));
			}

			State.Exists = true;

			return WriteStateAsync();
		}

		private object GetDefaultValue(Type type)
		{
			if (type.GetTypeInfo().IsValueType)
				return Activator.CreateInstance(type);
			else
				return null;
		}

		public Task Clear()
		{
			State = new MapDefinitionEntity();
			return ClearStateAsync();
		}
	}
}
