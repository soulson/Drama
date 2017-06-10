using System;
using System.Net.Sockets;

namespace Drama.Core.Gateway.Networking
{
  public class TcpSession
  {
    public Guid Id { get; }
    internal TcpServer Server { get; }
    internal Socket Socket { get; }

    public event EventHandler<DataReceivedEventArgs> DataReceived;
    public event EventHandler<DataSentEventArgs> DataSent;
    public event EventHandler<ClientDisconnectedEventArgs> Disconnected;

    public TcpSession(TcpServer server, Socket socket)
    {
      Server = server;
      Socket = socket;
      Id = Guid.NewGuid();
    }

    public void Send(byte[] data) => Server.Send(this, data);
    public void Send(byte[] data, int offset, int length) => Server.Send(this, data, offset, length);

    internal virtual void OnDataReceived(DataReceivedEventArgs e) => DataReceived?.Invoke(this, e);
    internal virtual void OnDataSent(DataSentEventArgs e) => DataSent?.Invoke(this, e);
    internal virtual void OnDisconnected(ClientDisconnectedEventArgs e) => Disconnected?.Invoke(this, e);
  }
}
