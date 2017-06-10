using System;
using System.Net.Sockets;

namespace Drama.Core.Gateway.Networking
{
  public class TcpSession
  {
    public Guid Id { get; }
    internal TcpServer Server { get; }
    internal Socket Socket { get; }

    public TcpSession(TcpServer server, Socket socket)
    {
      Server = server;
      Socket = socket;
      Id = Guid.NewGuid();
    }

    public void Send(byte[] data) => Server.Send(this, data);
    public void Send(byte[] data, int offset, int length) => Server.Send(this, data, offset, length);

    internal virtual void OnDataReceived(byte[] receivedData)
    {
    }

    internal virtual void OnDisconnected()
    {

    }
  }
}
