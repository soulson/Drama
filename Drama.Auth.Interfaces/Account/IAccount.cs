using Orleans;
using System;
using System.Numerics;
using System.Threading.Tasks;

namespace Drama.Auth.Interfaces.Account
{
	// a possible optimization would be to use an Integer-composite key with a Knuth hash
	//  as the primary and account name as the secondary
	public interface IAccount : IGrainWithStringKey
	{
		Task<bool> Exists();
		Task<AccountEntity> Create(string name, string password, AccountSecurityLevel securityLevel);
		Task<AccountEntity> GetEntity();
		Task<SrpInitialParameters> GetSrpInitialParameters();
		Task<SrpResult> SrpHandshake(BigInteger a, BigInteger m1);
	}
}
