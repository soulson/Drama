/* 
 * The Drama project: what you get when a bunch of actors try to host a game.
 * Copyright (C) 2017 Soulson
 *
 * This program is free software: you can redistribute it and/or modify
 * it under the terms of the GNU Affero General Public License as
 * published by the Free Software Foundation, either version 3 of the
 * License, or (at your option) any later version.
 *
 * This program is distributed in the hope that it will be useful,
 * but WITHOUT ANY WARRANTY; without even the implied warranty of
 * MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
 * GNU Affero General Public License for more details.
 *
 * You should have received a copy of the GNU Affero General Public License
 * along with this program.  If not, see <http://www.gnu.org/licenses/>.
 */

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
            ForwardPacket(await AuthSession.SubmitLogonChallenge(lc));
            break;
          case LogonProofRequest lp:
            ForwardPacket(await AuthSession.SubmitLogonProof(lp));
            break;
          case RealmListRequest rl:
            ForwardPacket(await AuthSession.GetRealmList(rl));
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

    public void ForwardPacket(IOutPacket packet)
    {
      using (var stream = new MemoryStream())
      {
        packet.Write(stream);
        Session.Send(stream.ToArray());
      }
    }
  }
}
