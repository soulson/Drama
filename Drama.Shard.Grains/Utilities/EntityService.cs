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

using Drama.Shard.Interfaces.Utilities;
using Orleans;
using Orleans.Concurrency;
using System;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

namespace Drama.Shard.Grains.Utilities
{
	[StatelessWorker]
	public class EntityService : Grain, IEntityService
	{
		public Task<TEntity> Merge<TEntity>(TEntity existing, TEntity @new)
		{
			var properties =
				from property in existing.GetType().GetProperties()
				where property.SetMethod != null
					 && property.GetMethod != null
				select property;

			foreach (var property in properties)
			{
				if (property.GetValue(@new) != GetDefaultValue(property.PropertyType))
					property.SetValue(existing, property.GetValue(@new));
			}

			return Task.FromResult(existing);
		}

		private object GetDefaultValue(Type type)
		{
			if (type.GetTypeInfo().IsValueType)
				return Activator.CreateInstance(type);
			else
				return null;
		}
	}
}
