using Drama.Core.Gateway.Networking;
using Drama.Core.Interfaces.Networking;
using Drama.Core.Interfaces.Utilities;
using Drama.Shard.Interfaces.Protocol;
using Drama.Shard.Interfaces.Session;
using Orleans;
using System;
using System.IO;
using System.Reflection;
using System.Threading.Tasks;

namespace Drama.Shard.Gateway
{
	// TODO: remove the Observer stuff from this class after using it as a reference implementation for ShardPacketRouter
	//  the auth protocol is strictly call-and-response
	public class ShardPacketRouter : PacketRouter, IShardSessionObserver
	{
		private const int InitialPacketCapacity = 32;

		private readonly byte[] twoZeroBytes;
    private readonly ShardPacketReader packetReader;
		private readonly ShardPacketCipher packetCipher;

    private IShardSessionObserver self;
    
    protected IGrainFactory GrainFactory { get; }
    protected IShardSession ShardSession { get; }

    public ShardPacketRouter(TcpSession session, IGrainFactory grainFactory) : base(session)
    {
      GrainFactory = grainFactory;
			ShardSession = GrainFactory.GetGrain<IShardSession>(Session.Id);
			twoZeroBytes = new byte[] { 0, 0 };
			packetCipher = new ShardPacketCipher();
			packetReader = new ShardPacketReader(packetCipher, typeof(ClientPacketAttribute).GetTypeInfo().Assembly);
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
					case PingRequest ping:
						ReceivePacket(new PongResponse() { Cookie = ping.Cookie });
						Console.WriteLine($"sent {ShardServerOpcode.Pong} with latency = {ping.Latency} and cookie = 0x{ping.Cookie:x8}");
						break;
          default:
						Console.WriteLine($"received an unimplemented packet: {packet.GetType().Name}");
						break;
        }
      }

			return Task.CompletedTask;
    }

    protected override Task OnSessionDisconnected(ClientDisconnectedEventArgs e)
    {
      return ShardSession.Disconnect(self);
    }

    public void ReceivePacket(IOutPacket packet)
    {
      using (var stream = new MemoryStream(InitialPacketCapacity))
      {
				// write a placeholder for the size of the packet. will set it properly in a moment
				stream.Write(twoZeroBytes, 0, twoZeroBytes.Length);

				// write the actual packet data (including the opcode)
        packet.Write(stream);

				// now write the real size (in big endian), excluding the size of the size
				stream.Position = 0;
				stream.Write(Arrays.Reverse(BitConverter.GetBytes(checked((ushort)(stream.Length - twoZeroBytes.Length)))), 0, sizeof(ushort));

				// encrypt the header of the packet
				var array = stream.ToArray();
				if (array.Length < ShardPacketCipher.SendLength)
					throw new ArgumentException($"packet is too short. must be at least {ShardPacketCipher.SendLength - twoZeroBytes.Length} bytes", nameof(packet));
				packetCipher.EncryptHeader(new ArraySegment<byte>(array, 0, ShardPacketCipher.SendLength));

				Session.Send(array);
      }
    }
  }
}
