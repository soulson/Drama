using Drama.Core.Interfaces.Networking;
using System;
using System.IO;
using System.Text;

namespace Drama.Auth.Interfaces.Protocol
{
	public sealed class RealmListResponse : IOutPacket
	{
		public void Write(Stream stream)
		{
			using (var writer = new BinaryWriter(stream, Encoding.UTF8, true))
			{
				writer.Write((byte)AuthRequestOpcode.RealmList);
				writer.Write((short)7); // number of remaining bytes
				writer.Write(0);
				writer.Write((byte)0); // number of shards in list
				writer.Write((short)2);
			}
		}
	}
}
