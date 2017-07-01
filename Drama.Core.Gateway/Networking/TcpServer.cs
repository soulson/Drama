using Drama.Core.Gateway.Utilities;
using System;
using System.Net;
using System.Net.Sockets;
using System.Threading;

namespace Drama.Core.Gateway.Networking
{
	// https://stackoverflow.com/questions/869744/how-to-write-a-scalable-tcp-ip-based-server/908766#908766
	// https://msdn.microsoft.com/en-us/library/system.net.sockets.socketasynceventargs.aspx
	// https://msdn.microsoft.com/en-us/library/bb517542.aspx
	public class TcpServer : IDisposable
  {
    private readonly DisposablePool<SocketAsyncEventArgs> socketEventPool;
    private readonly BufferPool bufferPool;
    private readonly Socket socket;

    private int activeSessions;

    public event EventHandler<ClientConnectedEventArgs> ClientConnected;
    public event EventHandler<ClientDisconnectedEventArgs> ClientDisconnected;
    public event EventHandler<DataReceivedEventArgs> DataReceived;
    public event EventHandler<DataSentEventArgs> DataSent;

    public int Backlog { get; }
    public bool Started { get; private set; }
    public bool Stopped { get; private set; }
    public IPEndPoint BindPoint { get; }
    public int BufferPoolBlockSize => bufferPool.BlockSize;
    public int BufferPoolBlockCount => bufferPool.BlockCount;
    public int BufferPoolLeaseCount => bufferPool.LeasedBlocks;
    public int ActiveSessions => activeSessions;

    public TcpServer(IPAddress bindAddress, int port, int connectionQueueSize, int bufferBlockSize, int bufferPoolSize)
    {
      Started = false;
      Stopped = false;
      Backlog = connectionQueueSize;
      BindPoint = new IPEndPoint(bindAddress ?? throw new ArgumentNullException(nameof(bindAddress)), port);
      activeSessions = 0;

      socketEventPool = new DisposablePool<SocketAsyncEventArgs>(() => new SocketAsyncEventArgs());
      bufferPool = new BufferPool(bufferBlockSize, bufferPoolSize);
      socket = new Socket(BindPoint.AddressFamily, SocketType.Stream, ProtocolType.Tcp);
    }

    public void Start()
    {
      if (Started)
        throw new InvalidOperationException($"{nameof(TcpServer)} cannot be started more than once");
      else
      {
        Started = true;
        OnStart();
      }
    }

    protected virtual void OnStart()
    {
      socket.Bind(BindPoint);
      socket.Listen(Backlog);
      StartAccept(null);
    }

    public void Stop()
    {
      if (Started)
      {
        if (Stopped)
          throw new InvalidOperationException($"{nameof(TcpServer)} cannot be stopped more than once");
        else
        {
          Stopped = true;
          OnStop();
        }
      }
      else
        throw new InvalidOperationException($"{nameof(TcpServer)} must be started before it can be stopped");
    }

    protected virtual void OnStop()
    {
      socket.Dispose();
    }

    private void StartAccept(SocketAsyncEventArgs e)
    {
      if (e == null)
      {
        e = socketEventPool.CheckOut();
        e.Completed += FinishAccept;
      }
      else
        e.AcceptSocket = null;

      try
      {
        if (!socket.AcceptAsync(e))
          FinishAccept(this, e);
      }
      catch (ObjectDisposedException)
      {
        e.Completed -= FinishAccept;
        socketEventPool.CheckIn(e);
        // TODO: log
      }
    }

    private void FinishAccept(object sender, SocketAsyncEventArgs e)
    {
      // FinishAccept seems to fire when the socket shuts down if an accept was pending, but the acceptsocket is not connected when this happens
      if (e.AcceptSocket != null && e.AcceptSocket.Connected)
      {
        e.AcceptSocket.NoDelay = true;

        var session = new TcpSession(this, e.AcceptSocket);
        Interlocked.Increment(ref activeSessions);
        OnClientConnected(new ClientConnectedEventArgs(session));

        var args = socketEventPool.CheckOut();
        args.UserToken = session;
        args.Completed += FinishReceive;
        StartReceive(args);

        StartAccept(e);
      }
      else
      {
        e.Completed -= FinishAccept;
        socketEventPool.CheckIn(e);
        // TODO: log
      }

    }

    protected virtual void OnClientConnected(ClientConnectedEventArgs e)
    {
      ClientConnected?.Invoke(this, e);
    }

    private void StartReceive(SocketAsyncEventArgs e)
    {
      if (e.UserToken is TcpSession session)
      {
        if (bufferPool.TryAssignBuffer(e))
        {
          if (!session.Socket.ReceiveAsync(e))
            FinishReceive(this, e);
        }
        else
          throw new InvalidOperationException("exceeded maximum available socket buffer partitions");
      }
    }

    private void FinishReceive(object sender, SocketAsyncEventArgs e)
    {
      var session = e.UserToken as TcpSession;

      if (e.BytesTransferred == 0 || e.SocketError != SocketError.Success || session == null)
      {
        bufferPool.FreeBuffer(e);
        Disconnect(e);
      }
      else
      {
        var receivedData = new byte[e.BytesTransferred];
        Buffer.BlockCopy(e.Buffer, e.Offset, receivedData, 0, receivedData.Length);
        bufferPool.FreeBuffer(e);

        var receiveEventArgs = new DataReceivedEventArgs(session, new ArraySegment<byte>(receivedData));
        OnDataReceived(receiveEventArgs);
        session.OnDataReceived(receiveEventArgs);

        StartReceive(e);
      }
    }

    protected virtual void OnDataReceived(DataReceivedEventArgs e)
    {
      DataReceived?.Invoke(this, e);
    }

    private void Disconnect(SocketAsyncEventArgs e)
    {
      if (e.UserToken is TcpSession session)
      {
        try
        {
          session.Socket.Shutdown(SocketShutdown.Both);
        }
        catch (SocketException)
        {
          // this seems to randomly happen when the client disconnects. not always.
          //  there are other examples online of this being caught and ignored like this.
          //  see links at the top of the file
          // TODO: log
        }

        session.Socket.Dispose();

        var disconnectEventArgs = new ClientDisconnectedEventArgs(session);
        OnClientDisconnected(disconnectEventArgs);
        session.OnDisconnected(disconnectEventArgs);

        Interlocked.Decrement(ref activeSessions);
      }

      if (e != null)
      {
        e.Completed -= FinishReceive;
        e.UserToken = null;
        e.SetBuffer(null, 0, 0);
        socketEventPool.CheckIn(e);
      }
    }

    protected virtual void OnClientDisconnected(ClientDisconnectedEventArgs e)
    {
      ClientDisconnected?.Invoke(this, e);
    }

    public void Send(TcpSession session, byte[] data) => Send(session, data, 0, data.Length);
    public void Send(TcpSession session, byte[] data, int offset, int length)
    {
      if (session == null)
        throw new ArgumentNullException(nameof(session));
      if (data == null)
        throw new ArgumentNullException(nameof(data));
      if (offset < 0 || offset > data.Length)
        throw new ArgumentOutOfRangeException(nameof(offset));
      if (length < 0 || length > data.Length)
        throw new ArgumentOutOfRangeException(nameof(length));

      var e = socketEventPool.CheckOut();
      e.UserToken = session;
      e.Completed += FinishSend;
      e.SetBuffer(data, offset, length);

      try
      {
        if (!session.Socket.SendAsync(e))
          FinishSend(this, e);
      }
      catch (ObjectDisposedException)
      {
        e.Completed -= FinishSend;
        e.SetBuffer(null, 0, 0);
        e.UserToken = null;
        socketEventPool.CheckIn(e);

        // TODO: log
      }
    }

    private void FinishSend(object sender, SocketAsyncEventArgs e)
    {
      var session = e.UserToken as TcpSession ?? throw new ArgumentNullException(nameof(e.UserToken));

      var sentEventArgs = new DataSentEventArgs(session, new ArraySegment<byte>(e.Buffer, e.Offset, e.BytesTransferred));
      OnDataSent(sentEventArgs);
      session.OnDataSent(sentEventArgs);

      e.Completed -= FinishSend;
      e.SetBuffer(null, 0, 0);
      e.UserToken = null;
      socketEventPool.CheckIn(e);
    }

    protected virtual void OnDataSent(DataSentEventArgs e)
    {
      DataSent?.Invoke(this, e);
    }

    #region IDisposable Support
    private bool disposedValue = false; // To detect redundant calls

    protected virtual void Dispose(bool disposing)
    {
      if (!disposedValue)
      {
        if (disposing)
        {
          // TODO: dispose managed state (managed objects).
          socket.Dispose();
          socketEventPool.Dispose();
        }

        // TODO: free unmanaged resources (unmanaged objects) and override a finalizer below.
        // TODO: set large fields to null.

        disposedValue = true;
      }
    }

    // TODO: override a finalizer only if Dispose(bool disposing) above has code to free unmanaged resources.
    // ~TcpServer() {
    //   // Do not change this code. Put cleanup code in Dispose(bool disposing) above.
    //   Dispose(false);
    // }

    // This code added to correctly implement the disposable pattern.
    public void Dispose()
    {
      // Do not change this code. Put cleanup code in Dispose(bool disposing) above.
      Dispose(true);
      // TODO: uncomment the following line if the finalizer is overridden above.
      // GC.SuppressFinalize(this);
    }
    #endregion
  }
}
