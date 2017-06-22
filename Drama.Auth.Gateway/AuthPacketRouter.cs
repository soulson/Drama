using Drama.Auth.Interfaces.Protocol;
using Drama.Auth.Interfaces.Session;
using Drama.Core.Gateway.Networking;
using Drama.Core.Interfaces.Networking;
using Orleans;
using System;
using System.IO;
using System.Threading.Tasks;

namespace Drama.Auth.Gateway
{
	public class AuthPacketRouter : PacketRouter
  {
    private readonly AuthPacketReader packetReader;
    
    protected IGrainFactory GrainFactory { get; }
    protected IAuthSession AuthSession { get; }

    public AuthPacketRouter(TcpSession session, IGrainFactory grainFactory) : base(session)
    {
      GrainFactory = grainFactory;
      AuthSession = GrainFactory.GetGrain<IAuthSession>(Session.Id);
      packetReader = new AuthPacketReader();
    }

    protected override Task OnInitialize()
    {
			return AuthSession.Connect();
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
            ReceivePacket(await AuthSession.SubmitLogonProof(lp));
            break;
          case RealmListRequest rl:
            ReceivePacket(await AuthSession.GetRealmList(rl));
            break;
          default:
            throw new Exception($"received unknown packet {packet}");
        }
      }
    }

    protected override Task OnSessionDisconnected(ClientDisconnectedEventArgs e)
    {
      return AuthSession.Disconnect();
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
