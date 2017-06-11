using Drama.Core.Interfaces.Networking;
using System;
using System.IO;

namespace Drama.Auth.Interfaces.Protocol
{
	public sealed class RealmListResponse : IOutPacket
	{
		public void Write(Stream stream)
		{
			throw new NotImplementedException();
		}
	}
}
