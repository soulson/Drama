using Drama.Core.Gateway.Networking;
using Drama.Core.Interfaces;
using Drama.Core.Interfaces.Networking;
using Drama.Core.Interfaces.Utilities;
using Drama.Shard.Interfaces.Protocol;
using Drama.Shard.Interfaces.Session;
using Orleans;
using System;
using System.Collections.Immutable;
using System.IO;
using System.Reflection;
using System.Threading.Tasks;

namespace Drama.Shard.Gateway
{
	public partial class ShardPacketRouter : PacketRouter, IShardSessionObserver
	{
		private const int InitialPacketCapacity = 32;

		private readonly byte[] twoZeroBytes;
		private readonly string shardName;
    private readonly ShardPacketReader packetReader;
		private readonly ShardPacketCipher packetCipher;

    private IShardSessionObserver self;
		private bool authenticationFailed;
    
    protected IGrainFactory GrainFactory { get; }
    protected IShardSession ShardSession { get; }
		protected IImmutableDictionary<Type, MethodInfo> PacketHandlers { get; }

		[AttributeUsage(AttributeTargets.Method, AllowMultiple = true, Inherited = true)]
		protected sealed class HandlerAttribute : Attribute
		{
			public Type PacketType { get; }

			public HandlerAttribute(Type packetType)
				=> PacketType = packetType;
		}

    public ShardPacketRouter(TcpSession session, IGrainFactory grainFactory, string shardName) : base(session)
    {
      GrainFactory = grainFactory;
			ShardSession = GrainFactory.GetGrain<IShardSession>(Session.Id);
			twoZeroBytes = new byte[] { 0, 0 };
			packetCipher = new ShardPacketCipher();
			packetReader = new ShardPacketReader(packetCipher, typeof(ClientPacketAttribute).GetTypeInfo().Assembly);
			authenticationFailed = false;
			this.shardName = shardName;

			// populate the packet handler map
			var handlerMapBuilder = ImmutableDictionary.CreateBuilder<Type, MethodInfo>();
			var typeInfo = GetType().GetTypeInfo();

			foreach (var method in typeInfo.DeclaredMethods)
			{
				foreach (var handlerAttribute in method.GetCustomAttributes<HandlerAttribute>(true))
				{
					if (method.ReturnType != typeof(Task))
						throw new DramaException($"packet handler method {method.Name} must return {nameof(Task)}");

					handlerMapBuilder.Add(handlerAttribute.PacketType, method);
				}
			}

			PacketHandlers = handlerMapBuilder.ToImmutable();
    }

    protected override Task OnInitialize()
    {
      if (self == null)
      {
        self = GrainFactory.CreateObjectReference<IShardSessionObserver>(this).Result;
        return ShardSession.Connect(self, shardName);
      }
      else
        throw new InvalidOperationException("cannot initialize more than once");
    }

		protected override async Task OnSessionDataReceived(DataReceivedEventArgs e)
		{
			// if the session failed to authenticate, it cannot be recovered. just discard all incoming data
			if (!authenticationFailed)
			{
				foreach (var packet in packetReader.ProcessData(e.ReceivedData))
				{
					var packetType = packet.GetType();

					if (PacketHandlers.ContainsKey(packetType))
					{
						var handlerMethod = PacketHandlers[packetType];
						await (Task)handlerMethod.Invoke(this, new object[] { packet });
					}
					else
						Console.WriteLine($"received an unimplemented packet: {packet.GetType().Name}");
				}
			}
		}

    protected override Task OnSessionDisconnected(ClientDisconnectedEventArgs e)
    {
      return ShardSession.Disconnect(self);
    }

    public void ForwardPacket(IOutPacket packet)
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
