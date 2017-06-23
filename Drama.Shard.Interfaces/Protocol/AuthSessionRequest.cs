using Drama.Core.Interfaces.Networking;
using Drama.Core.Interfaces.Utilities;
using System.IO;
using System.Numerics;
using System.Text;

namespace Drama.Shard.Interfaces.Protocol
{
	[ClientPacket(ShardClientOpcode.AuthSession)]
	public sealed class AuthSessionRequest : IInPacket
	{
		public int ClientBuild { get; set; }
		public string Identity { get; set; }
		public int ClientSeed { get; set; }
		public BigInteger ClientDigest { get; set; }

		public bool Read(Stream stream)
		{
			try
			{
				using (var reader = new BinaryReader(stream, Encoding.UTF8, true))
				{
					ClientBuild = reader.ReadInt32();

					stream.Seek(4, SeekOrigin.Current);
					Identity = reader.ReadNullTerminatedString(Encoding.UTF8);
					ClientSeed = reader.ReadInt32();
					ClientDigest = reader.ReadBigInteger(20);
				}

				return true;
			}
			catch (EndOfStreamException)
			{
				return false;
			}
		}
	}
}
