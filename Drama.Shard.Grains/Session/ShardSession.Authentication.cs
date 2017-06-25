using Drama.Auth.Interfaces;
using Drama.Auth.Interfaces.Account;
using Drama.Core.Interfaces.Security;
using Drama.Core.Interfaces.Utilities;
using Drama.Shard.Interfaces.Protocol;
using Drama.Shard.Interfaces.Session;
using Orleans;
using System;
using System.Numerics;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace Drama.Shard.Grains.Session
{
	public partial class ShardSession
	{
		public async Task<BigInteger> Authenticate(AuthSessionRequest authRequest)
		{
			if (String.IsNullOrEmpty(authRequest.Identity))
				throw new ArgumentNullException(nameof(authRequest.Identity));

			if (seed == 0)
			{
				await Send(new AuthSessionResponse() { Response = AuthResponseCode.Failed });
				throw new AuthenticationFailedException("cannot authenticate with a server seed of 0");
			}

			var account = GrainFactory.GetGrain<IAccount>(authRequest.Identity);

			if (await account.Exists())
			{
				try
				{
					var sessionKey = await account.GetSessionKey();

					using (var sha1 = new Digester(SHA1.Create()))
					{
						var serverDigest = BigIntegers.FromUnsignedByteArray(
							sha1.CalculateDigest(new byte[][]
							{
								Encoding.UTF8.GetBytes(authRequest.Identity),
								new byte[4],
								BitConverter.GetBytes(authRequest.ClientSeed),
								BitConverter.GetBytes(seed),
								sessionKey.ToByteArray(40),
							})
						);

						if (serverDigest == authRequest.ClientDigest)
						{
							GetLogger().Info($"{authRequest.Identity} successfully authenticated to {nameof(ShardSession)} {this.GetPrimaryKey()}");

							// we can't just Send the Success response here, since the client expects the packet cipher to be initialized at this point
							AuthenticatedIdentity = authRequest.Identity;
							return sessionKey;
						}
						else
						{
							await Send(new AuthSessionResponse() { Response = AuthResponseCode.Failed });
							throw new AuthenticationFailedException($"account {authRequest.Identity} failed authentication proof");
						}
					}
				}
				catch (AccountDoesNotExistException)
				{
					await Send(new AuthSessionResponse() { Response = AuthResponseCode.UnknownAccount });
					throw new AuthenticationFailedException($"account {authRequest.Identity} does not exist");
				}
				catch (AccountStateException)
				{
					GetLogger().Warn($"received {nameof(AuthSessionRequest)} with unauthenticated identity {authRequest.Identity}");
					await Send(new AuthSessionResponse() { Response = AuthResponseCode.Failed });
					throw new AuthenticationFailedException($"account {authRequest.Identity} is not authenticated");
				}
			}
			else
			{
				await Send(new AuthSessionResponse() { Response = AuthResponseCode.UnknownAccount });
				throw new AuthenticationFailedException($"account {authRequest.Identity} does not exist");
			}
		}

		public Task Handshake(AuthSessionRequest authRequest)
		{
			if (AuthenticatedIdentity == authRequest.Identity)
				return Task.WhenAll(Send(new AuthSessionResponse() { Response = AuthResponseCode.Success }), SendAddonPacket(authRequest));
			else
				throw new SessionStateException($"received {nameof(Handshake)} request for identity {authRequest.Identity} but {nameof(AuthenticatedIdentity)} is {AuthenticatedIdentity}");
		}

		private Task SendAddonPacket(AuthSessionRequest authRequest)
		{
			const int StandardAddonCRC = 0x1c776d01;
			var response = new AddonInfoResponse();

			foreach (var block in authRequest.AddonBlocks)
				response.IsAddonStandard.Add(block.Crc == StandardAddonCRC);

			return Send(response);
		}
	}
}
