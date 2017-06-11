using Drama.Auth.Interfaces.Account;
using Orleans;
using Orleans.Providers;
using System;
using System.Threading.Tasks;

namespace Drama.Auth.Grains.Account
{
	[StorageProvider(ProviderName = "AccountStore")]
	public class Account : Grain<AccountEntity>, IAccount
	{
		public Task<AccountEntity> Create(string name, string password, AccountSecurityLevel securityLevel)
		{
			throw new NotImplementedException();
		}

		public Task<bool> Exists() => Task.FromResult(State.Enabled);

		public Task<AccountEntity> GetEntity()
		{
			throw new NotImplementedException();
		}
	}
}
