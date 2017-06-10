using System;

namespace Drama.Core.Gateway.Networking
{
  public class ClientConnectedEventArgs : EventArgs
  {
    public TcpSession Session { get; }

    public ClientConnectedEventArgs(TcpSession session)
    {
      Session = session;
    }
  }
}
