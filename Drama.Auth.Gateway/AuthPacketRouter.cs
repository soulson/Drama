using Drama.Auth.Interfaces.Packets;
using Drama.Auth.Interfaces.Session;
using Drama.Core.Gateway.Networking;
using Drama.Core.Interfaces.Networking;
using Orleans;
using System;
using System.IO;
using System.Threading.Tasks;

namespace Drama.Auth.Gateway
{
	// TODO: remove the Observer stuff from this class after using it as a reference implementation for ShardPacketRouter
	//  the auth protocol is strictly call-and-response
	public class AuthPacketRouter : PacketRouter, IAuthSessionObserver
  {
    private readonly AuthPacketReader packetReader;

    private IAuthSessionObserver self;
    
    protected IGrainFactory GrainFactory { get; }
    protected IAuthSession AuthSession { get; }

    public AuthPacketRouter(TcpSession session, IGrainFactory grainFactory) : base(session)
    {
      GrainFactory = grainFactory;
      AuthSession = GrainFactory.GetGrain<IAuthSession>(Session.Id);
      packetReader = new AuthPacketReader();
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

    protected override async Task OnSessionDataReceived(DataReceivedEventArgs e)
    {
      foreach(var packet in packetReader.ProcessData(e.ReceivedData))
      {
        switch (packet)
        {
          case LogonChallengeRequest lc:
            ReceivePacket(await AuthSession.SubmitLogonChallenge(lc));
            break;
          case LogonProofRequest lp:
            await AuthSession.SubmitLogonProof(lp);
            break;
          case RealmListRequest rl:
            await AuthSession.GetRealmList(rl);
            break;
          default:
            throw new Exception($"received unknown packet {packet}");
        }
      }
    }

    protected override async Task OnSessionDisconnected(ClientDisconnectedEventArgs e)
    {
      await AuthSession.Disconnect(self);
    }

    public void ReceivePacket(IOutPacket packet)
    {
      using (var stream = new MemoryStream())
      {
        packet.Write(stream);
        Session.Send(stream.ToArray());
      }
    }
  }
}
