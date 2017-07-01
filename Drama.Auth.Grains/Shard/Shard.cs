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
using System.Threading.Tasks;

namespace Drama.Auth.Grains.Shard
{
	[StorageProvider(ProviderName = StorageProviders.Infrastructure)]
	public class Shard : Grain<ShardEntity>, IShard
	{
		// private synchronous version of Exists()
		private bool IsExists => State.Enabled;

		public async Task<ShardEntity> Create(string address, int port, ShardType type, ShardFlags flags)
		{
			if (IsExists)
				throw new ShardExistsException($"shard {this.GetPrimaryKeyString()} already exists; cannot create shard");

			State.Name = this.GetPrimaryKeyString();
			State.Address = address ?? throw new ArgumentNullException(nameof(address));
			State.Port = port;
			State.ShardType = type;
			State.ShardFlags = flags;
			State.Enabled = true;

			await WriteStateAsync();

			// orleans will safe-copy this automatically
			return State;
		}

		public Task<bool> Exists() => Task.FromResult(IsExists);

		public Task<ShardEntity> GetEntity()
		{
			if (!IsExists)
				throw new ShardDoesNotExistException($"shard {this.GetPrimaryKeyString()} does not exist");

			return Task.FromResult(State);
		}
	}
}
