using System;

namespace Drama.Core.Gateway.Networking
{
  public class DataReceivedEventArgs : EventArgs
  {
    public TcpSession Session { get; }
    public ArraySegment<byte> ReceivedData { get; }

    public DataReceivedEventArgs(TcpSession session, ArraySegment<byte> receivedData)
    {
      Session = session;
      ReceivedData = receivedData;
    }
  }
}
