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
