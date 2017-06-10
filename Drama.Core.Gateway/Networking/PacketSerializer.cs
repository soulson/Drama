using System;
using System.Threading;
using System.Threading.Tasks;

namespace Drama.Core.Gateway.Networking
{
  public abstract class PacketSerializer
  {
    private readonly ManualResetEvent initialized;

    protected TcpSession Session { get; }

    private readonly EventHandler<DataReceivedEventArgs> sessionDataReceived;
    private readonly EventHandler<ClientDisconnectedEventArgs> sessionDisconnected;

    public PacketSerializer(TcpSession session)
    {
      Session = session;
      initialized = new ManualResetEvent(false);

      // session events are raised in IOCP threads—under Windows, at least—so it's important not to wait within their handlers
      sessionDataReceived = async (sender, e) => await SessionDataReceivedAsync(sender, e);
      sessionDisconnected = async (sender, e) => await SessionDisconnectedAsync(sender, e);
      Session.DataReceived += sessionDataReceived;
      Session.Disconnected += sessionDisconnected;
    }

    public async Task InitializeAsync()
    {
      await OnInitialize();
      initialized.Set();
    }

    protected abstract Task OnInitialize();

    private async Task SessionDataReceivedAsync(object sender, DataReceivedEventArgs e)
    {
      WaitForInitialization();
      await OnSessionDataReceived(e);
    }

    protected abstract Task OnSessionDataReceived(DataReceivedEventArgs e);

    private async Task SessionDisconnectedAsync(object sender, ClientDisconnectedEventArgs e)
    {
      // waiting for initialization proves self is non-null
      WaitForInitialization();
      await OnSessionDisconnected(e);

      // need to clean up event hooks here so that this object can be GC'd
      Session.DataReceived -= sessionDataReceived;
      Session.Disconnected -= sessionDisconnected;

      initialized.Dispose();
    }

    protected abstract Task OnSessionDisconnected(ClientDisconnectedEventArgs e);

    protected void WaitForInitialization()
    {
      // we need to wait until InitializeAsync finishes before sending any data to the host, or we may not
      //  be able to read the response
      if (!initialized.WaitOne(TimeSpan.FromSeconds(16)))
        throw new InvalidOperationException($"{nameof(PacketSerializer)} was not initialized");
    }
  }
}
