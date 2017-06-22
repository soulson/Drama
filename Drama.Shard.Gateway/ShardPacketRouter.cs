using Drama.Core.Gateway.Networking;
using Drama.Core.Interfaces.Networking;
using Drama.Shard.Interfaces.Session;
using Orleans;
using System;
using System.IO;
using System.Threading.Tasks;

namespace Drama.Shard.Gateway
{
	// TODO: remove the Observer stuff from this class after using it as a reference implementation for ShardPacketRouter
	//  the auth protocol is strictly call-and-response
	public class ShardPacketRouter : PacketRouter, IShardSessionObserver
	{
    private readonly ShardPacketReader packetReader;
		private readonly ShardPacketCipher packetCipher;

    private IShardSessionObserver self;
    
    protected IGrainFactory GrainFactory { get; }
    protected IShardSession ShardSession { get; }

    public ShardPacketRouter(TcpSession session, IGrainFactory grainFactory) : base(session)
    {
      GrainFactory = grainFactory;
			ShardSession = GrainFactory.GetGrain<IShardSession>(Session.Id);
			packetCipher = new ShardPacketCipher();
			packetReader = new ShardPacketReader(packetCipher);
    }

    protected override async Task OnInitialize()
    {
      if (self == null)
      {
        self = await GrainFactory.CreateObjectReference<IShardSessionObserver>(this);
        await ShardSession.Connect(self);
      }
      else
        throw new InvalidOperationException("cannot initialize more than once");
    }

    protected override Task OnSessionDataReceived(DataReceivedEventArgs e)
    {
      foreach(var packet in packetReader.ProcessData(e.ReceivedData))
      {
        switch (packet)
        {
          default:
						Console.WriteLine($"received an unimplemented packet: {packet.GetType().Name}");
						break;
        }
      }

			return Task.CompletedTask;
    }

    protected override async Task OnSessionDisconnected(ClientDisconnectedEventArgs e)
    {
      await ShardSession.Disconnect(self);
    }

    public void ReceivePacket(IOutPacket packet)
    {
      using (var stream = new MemoryStream())
      {
        packet.Write(stream);

				// encrypt the header of the packet
				var array = stream.ToArray();
				if (array.Length < ShardPacketCipher.SendLength)
					throw new ArgumentException($"packet is too short. must be at least {ShardPacketCipher.SendLength} bytes", nameof(packet));
				packetCipher.EncryptHeader(new ArraySegment<byte>(array, 0, ShardPacketCipher.SendLength));

				Session.Send(array);
      }
    }
  }
}
