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

using Drama.Auth.Interfaces.Shard;
using Drama.Core.Interfaces;
using Orleans;
using Orleans.Providers;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Drama.Auth.Grains.Shard
{
	[StorageProvider(ProviderName = StorageProviders.Infrastructure)]
	public class ShardList : Grain<List<string>>, IShardList
	{
		public Task AddShardKey(string key)
		{
			if (!State.Contains(key))
				State.Add(key);

			return WriteStateAsync();
		}

		public Task AddShardKey(string key, string ip, int port)
		{
			throw new NotImplementedException();
		}

		public Task<IList<string>> GetShardKeys()
			=> Task.FromResult<IList<string>>(State);
	}
}
