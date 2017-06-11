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
		private BigInteger BPublic { get; set; }
		private BigInteger BPrivate { get; set; }
		private BigInteger SessionKey { get; set; }

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

		public Task<AccountEntity> GetEntity()
		{
			if (!IsExists)
				throw new AccountDoesNotExistException($"account {this.GetPrimaryKeyString()} does not exist");

			return Task.FromResult(State.Clone());
		}

		public async Task<SrpInitialParameters> GetSrpInitialParameters()
		{
			if (!IsExists)
				throw new AccountDoesNotExistException($"account {this.GetPrimaryKeyString()} does not exist");

			var random = GrainFactory.GetGrain<IRandomService>(0);
			var randomNumber = await random.GetRandomBigInteger(16);

			BPrivate = await random.GetRandomBigInteger(19);
			BPublic = ((State.Verifier * K) + BigInteger.ModPow(G, BPrivate, N)) % N;

			return new SrpInitialParameters(BPublic, G, N, State.Salt, randomNumber);
		}

		public Task<SrpResult> SrpHandshake(BigInteger a, BigInteger m1)
		{
			if (!IsExists)
				throw new AccountDoesNotExistException($"account {this.GetPrimaryKeyString()} does not exist");
			if (a.IsZero)
				throw new SrpException("A cannot be zero");
			if (BPublic.IsZero)
				throw new SrpException("B cannot be zero");

			using (var sha1 = new Digester(SHA1.Create()))
			{
				var u = BigIntegers.FromUnsignedByteArray(sha1.CalculateDigest(a, BPublic));
				var s = BigInteger.ModPow(a * BigInteger.ModPow(State.Verifier, u, N), BPrivate, N);

				var t = s.ToByteArray(32);
				var t1 = new byte[16];
				var vK = new byte[40];

				// modified srp - uses a session key (vK, K) created by interleaving two hashes (tK) created from half (t1) of the shared secret (s)
				for (int i = 0; i < 16; ++i)
					t1[i] = t[i * 2];
				var tK = sha1.CalculateDigest(t1);
				for (int i = 0; i < 20; ++i)
					vK[i * 2] = tK[i];

				for (int i = 0; i < 16; ++i)
					t1[i] = t[i * 2 + 1];
				tK = sha1.CalculateDigest(t1);
				for (int i = 0; i < 20; ++i)
					vK[i * 2 + 1] = tK[i];

				SessionKey = BigIntegers.FromUnsignedByteArray(vK);

				var Nghash = sha1.CalculateDigest(N.ToByteArray(32));
				var ghash = sha1.CalculateDigest(new[] { G });

				for (int i = 0; i < sha1.DigestSize; ++i)
					Nghash[i] ^= ghash[i];

				var serverM1Bytes = sha1.CalculateDigest(new byte[][]
				{
					Nghash,
					sha1.CalculateDigest(State.Name),
					State.Salt.ToByteArray(32),
					a.ToByteArray(32),
					BPublic.ToByteArray(32),
					vK,
				});

				// if client M1 matches server M1, then client and server have agreed on shared SessionKey, but only server knows that right now
				var serverM1 = BigIntegers.FromUnsignedByteArray(serverM1Bytes);
				if (serverM1 == m1)
				{
					// need to send M2 so client knows session key is shared also
					var m2Bytes = sha1.CalculateDigest(new byte[][]
					{
						a.ToByteArray(32),
						serverM1Bytes,
						SessionKey.ToByteArray(40),
					});

					var m2 = BigIntegers.FromUnsignedByteArray(m2Bytes);
					return Task.FromResult(new SrpResult(true, m2));
				}
				else
					return Task.FromResult(new SrpResult(false, BigInteger.Zero));
			}
		}
	}
}
