using Drama.Core.Interfaces.Networking;
using System;
using System.IO;
using System.Text;

namespace Drama.Auth.Interfaces.Packets
{
  public sealed class LogonChallenge : IPacket
  {
    public string Identity { get; set; }

    public bool Read(Stream stream)
    {
      stream.Seek(33, SeekOrigin.Current);
      var identityLength = stream.ReadByte();

      if (identityLength < 0)
        return false;
      if (identityLength + stream.Position > stream.Length)
        return false;

      var identityBytes = new byte[identityLength];
      stream.Read(identityBytes, 0, identityBytes.Length);

      Identity = Encoding.UTF8.GetString(identityBytes);

      return true;
    }

    public void Write(Stream stream) => throw new NotImplementedException();
  }
}
