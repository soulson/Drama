using Drama.Auth.Interfaces.Account;
using Drama.Auth.Interfaces.Utilities;
using Drama.Core.Interfaces.Security;
using Drama.Core.Interfaces.Utilities;
using Orleans;
using Orleans.Providers;
using System;
using System.Globalization;
using System.Numerics;
using System.Security.Cryptography;
using System.Threading.Tasks;

namespace Drama.Auth.Grains.Account
{
	[StorageProvider(ProviderName = "AccountStore")]
	public class Account : Grain<AccountEntity>, IAccount
	{
		// SRP6 constants
		private const byte G = 7;
		private const byte K = 3;
		private static readonly BigInteger N = BigInteger.Parse("0894B645E89E1535BBDAD5B8B290650530801B18EBFBF5E8FAB3C82872A3E9BB7", NumberStyles.AllowHexSpecifier);

		// synchronous private version of Exists()
		private bool IsExists => State.Enabled;
		private BigInteger BPrivate { get; set; }

		public async Task<AccountEntity> Create(string name, string password, AccountSecurityLevel securityLevel)
		{
			name = name?.ToUpperInvariant() ?? throw new ArgumentNullException(nameof(name));
			password = password?.ToUpperInvariant() ?? throw new ArgumentNullException(nameof(password));

			if (IsExists)
				throw new AccountExistsException($"account {State.Name} already exists, cannot create account {name}");

			var random = GrainFactory.GetGrain<IRandomService>(0);

			using (var sha1 = new Digester(SHA1.Create()))
			{
				var salt = await random.GetRandomBigInteger(32);
				var identityHash = sha1.CalculateDigest($"{name}:{password}");
				var secret = BigIntegers.FromUnsignedByteArray(sha1.CalculateDigest(new byte[][] { salt.ToByteArray(32), identityHash }));
				var verifier = BigInteger.ModPow(G, secret, N);

				State.Salt = salt;
				State.Verifier = verifier;
				State.Name = name;
				State.Enabled = true;
				State.SecurityLevel = securityLevel;
			}

			await WriteStateAsync();

			return State.Clone();
		}

		public Task<bool> Exists() => Task.FromResult(IsExists);

		public Task<AccountEntity> GetEntity() => Task.FromResult(State.Clone());

		public async Task<SrpInitialParameters> GetSrpInitialParameters()
		{
			if (!IsExists)
				throw new AccountDoesNotExistException($"account {this.GetPrimaryKeyString()} does not exist");

			var random = GrainFactory.GetGrain<IRandomService>(0);
			var randomNumber = await random.GetRandomBigInteger(16);

			BPrivate = await random.GetRandomBigInteger(19);
			var B = ((State.Verifier * K) + BigInteger.ModPow(G, BPrivate, N)) % N;

			return new SrpInitialParameters(B, G, N, State.Salt, randomNumber);
		}
	}
}
