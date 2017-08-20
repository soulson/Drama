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
using Microsoft.EntityFrameworkCore;
using Orleans;
using System;

namespace Drama.Tools.Load.Formats.Sql
{
	public class SqlLoader<TGrain, TGrainEntity, TSqlEntity>
		where TGrain : IGrainWithIntegerKey, IMergeable<TGrainEntity>
		where TGrainEntity : new()
		where TSqlEntity : class, ISqlEntity<TGrainEntity>
	{
		private readonly DbContext context;

		public SqlLoader(DbContext context)
		{
			this.context = context ?? throw new ArgumentNullException(nameof(context));
		}

		public void LoadEntities(IGrainFactory grainFactory)
		{
			foreach (var row in context.Set<TSqlEntity>())
			{
				var idValue = row.GetKey();
				var grain = grainFactory.GetGrain<TGrain>(idValue);
				var grainEntity = row.ToGrainEntity();

				grain.Clear().Wait();
				grain.Merge(grainEntity).Wait();
			}
		}
	}
}
