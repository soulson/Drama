﻿using Drama.Core.Interfaces.Networking;
using System;
using System.IO;
using System.Text;

namespace Drama.Auth.Interfaces.Protocol
{
  public sealed class LogonChallengeRequest : IInPacket
  {
    public string Identity { get; set; }

    public bool Read(Stream stream)
    {
      stream.Seek(32, SeekOrigin.Current);
      var identityLength = stream.ReadByte();

      if (identityLength < 0)
        return false;
      if (identityLength + stream.Position > stream.Length)
        return false;

      var identityBytes = new byte[identityLength];
      var bytesRead = stream.Read(identityBytes, 0, identityBytes.Length);

			if (bytesRead < identityLength)
				return false;

      Identity = Encoding.UTF8.GetString(identityBytes);

      return true;
    }
  }
}
