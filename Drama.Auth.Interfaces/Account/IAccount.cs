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

using Orleans;
using System.Numerics;
using System.Threading.Tasks;

namespace Drama.Auth.Interfaces.Account
{
	// a possible optimization would be to use an Integer-composite key with a Knuth hash
	//  as the primary and account name as the secondary
	public interface IAccount : IGrainWithStringKey
	{
		Task<bool> Exists();
		Task<AccountEntity> Create(string password, AccountSecurityLevel securityLevel);
		Task<AccountEntity> GetEntity();
		Task<SrpInitialParameters> GetSrpInitialParameters();
		Task<SrpResult> SrpHandshake(BigInteger a, BigInteger m1);
		Task Deauthenticate();
		Task<BigInteger> GetSessionKey();
	}
}
