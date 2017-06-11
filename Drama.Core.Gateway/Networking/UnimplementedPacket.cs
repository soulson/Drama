using Drama.Core.Interfaces.Networking;
using System;
using System.IO;

namespace Drama.Core.Gateway.Networking
{
  public sealed class UnimplementedPacket : IInPacket
  {
    public int Size { get; }

    public UnimplementedPacket(int size) => Size = size;

    public bool Read(Stream stream)
    {
      if (stream.Length - stream.Position < Size)
        return false;

      stream.Seek(Size, SeekOrigin.Current);
      return true;
    }

    public void Write(Stream stream) => throw new NotImplementedException();
  }
}
