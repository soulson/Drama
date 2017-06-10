using Drama.Auth.Interfaces;
using Drama.Core.Gateway.Networking;
using Drama.Core.Interfaces.Networking;
using Orleans;
using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace Drama.Auth.Gateway
{
  public class PacketSerializer : IAuthSessionObserver
  {
    private readonly ManualResetEvent initialized;

    private IAuthSessionObserver self;

    protected TcpSession Session { get; }
    protected IGrainFactory GrainFactory { get; }
    protected IAuthSession AuthSession { get; }

    private readonly EventHandler<DataReceivedEventArgs> sessionDataReceived;
    private readonly EventHandler<ClientDisconnectedEventArgs> sessionDisconnected;

    public PacketSerializer(TcpSession session, IGrainFactory grainFactory)
    {
      Session = session;
      GrainFactory = grainFactory;
      AuthSession = GrainFactory.GetGrain<IAuthSession>(Session.Id);
      initialized = new ManualResetEvent(false);

      // session events are raised in IOCP threads—under Windows, at least—so it's important not to wait within their handlers
      sessionDataReceived = async (sender, e) => await OnSessionDataReceivedAsync(sender, e);
      sessionDisconnected = async (sender, e) => await OnSessionDisconnectedAsync(sender, e);
      Session.DataReceived += sessionDataReceived;
      Session.Disconnected += sessionDisconnected;
    }

    public async Task InitializeAsync()
    {
      if (self == null)
      {
        self = await GrainFactory.CreateObjectReference<IAuthSessionObserver>(this);
        await AuthSession.Connect(self);
        initialized.Set();
      }
      else
        throw new InvalidOperationException("cannot initialize more than once");
    }

    protected virtual async Task OnSessionDataReceivedAsync(object sender, DataReceivedEventArgs e)
    {
      WaitForInitialization();
      Console.WriteLine($"received {e.ReceivedData.Count} bytes of data in PacketSerializer!");
    }

    protected virtual async Task OnSessionDisconnectedAsync(object sender, ClientDisconnectedEventArgs e)
    {
      // waiting for initialization proves self is non-null
      WaitForInitialization();
      await AuthSession.Disconnect(self);

      // need to clean up event hooks here so that this object can be GC'd
      Session.DataReceived -= sessionDataReceived;
      Session.Disconnected -= sessionDisconnected;

      initialized.Dispose();
    }

    public void ReceivePacket(IPacket packet)
    {
      using (var stream = new MemoryStream())
      {
        packet.Write(stream);
        Session.Send(stream.ToArray());
      }
    }

    private void WaitForInitialization()
    {
      // we need to wait until InitializeAsync finishes before sending any data to the host, or we may not
      //  be able to read the response
      if (!initialized.WaitOne(TimeSpan.FromSeconds(16)))
        throw new InvalidOperationException($"{nameof(PacketSerializer)} was not initialized");
    }
  }
}
