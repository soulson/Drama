using System;

namespace Drama.Core.Gateway.Networking
{
  public class ClientDisconnectedEventArgs : EventArgs
  {
    public TcpSession Session { get; }

    public ClientDisconnectedEventArgs(TcpSession session)
    {
      Session = session;
    }
  }
}
