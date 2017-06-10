using System;

namespace Drama.Core.Gateway.Networking
{
  public class DataSentEventArgs : EventArgs
  {
    public TcpSession Session { get; }
    public ArraySegment<byte> SentData { get; }

    public DataSentEventArgs(TcpSession session, ArraySegment<byte> sentData)
    {
      Session = session;
      SentData = sentData;
    }
  }
}
