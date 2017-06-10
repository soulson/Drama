using Drama.Auth.Interfaces;
using Drama.Core.Gateway.Networking;
using Drama.Core.Interfaces.Networking;
using Orleans;
using System;
using System.IO;
using System.Threading.Tasks;

namespace Drama.Auth.Gateway
{
  public class AuthPacketFilter : PacketFilter, IAuthSessionObserver
  {
    private IAuthSessionObserver self;
    
    protected IGrainFactory GrainFactory { get; }
    protected IAuthSession AuthSession { get; }

    public AuthPacketFilter(TcpSession session, IGrainFactory grainFactory) : base(session)
    {
      GrainFactory = grainFactory;
      AuthSession = GrainFactory.GetGrain<IAuthSession>(Session.Id);
    }

    protected override async Task OnInitialize()
    {
      if (self == null)
      {
        self = await GrainFactory.CreateObjectReference<IAuthSessionObserver>(this);
        await AuthSession.Connect(self);
      }
      else
        throw new InvalidOperationException("cannot initialize more than once");
    }

    protected override Task OnSessionDataReceived(DataReceivedEventArgs e)
    {
      Console.WriteLine($"received {e.ReceivedData.Count} bytes of data in PacketSerializer!");
      return Task.CompletedTask;
    }

    protected override async Task OnSessionDisconnected(ClientDisconnectedEventArgs e)
    {
      await AuthSession.Disconnect(self);
    }

    public void ReceivePacket(IPacket packet)
    {
      using (var stream = new MemoryStream())
      {
        packet.Write(stream);
        Session.Send(stream.ToArray());
      }
    }
  }
}
