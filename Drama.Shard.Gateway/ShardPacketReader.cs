﻿using Drama.Shard.Interfaces.Protocol;
using Drama.Core.Gateway.Networking;
using Drama.Core.Interfaces.Networking;
using System;
using System.IO;
using System.Linq;
using System.Collections.Immutable;
using System.Reflection;
using System.Collections.Generic;

namespace Drama.Shard.Gateway
{
	public class ShardPacketReader : PacketReader
  {
		private const int ClientPacketHeaderSize = 6;

		private readonly ShardPacketCipher packetCipher;
		private readonly ImmutableDictionary<ShardClientOpcode, Type> packetMap;

		public ShardPacketReader(ShardPacketCipher packetCipher, Assembly packetDefinitionAssembly)
		{
			this.packetCipher = packetCipher;

			var annotatedTypes =
				from type in packetDefinitionAssembly.GetExportedTypes()
				where type.GetTypeInfo().GetCustomAttribute<ClientPacketAttribute>() != null
				select new KeyValuePair<ShardClientOpcode, Type>(type.GetTypeInfo().GetCustomAttribute<ClientPacketAttribute>().Opcode, type);

			packetMap = ImmutableDictionary.CreateRange(annotatedTypes);
		}

    protected override IInPacket CreatePacket(Stream stream)
    {
			if (stream.Length - stream.Position < ClientPacketHeaderSize)
				return null;

			var initialPosition = stream.Position;
			var header = new byte[ClientPacketHeaderSize];
			stream.Read(header, 0, header.Length);

			packetCipher.DecryptHeader(new ArraySegment<byte>(header));

			// the size is encoded in big endian and doesn't include the size of the size itself
			var size = sizeof(ushort) + BitConverter.ToUInt16(new[] { header[1], header[0] }, 0);
			var ordinal = BitConverter.ToUInt32(header, sizeof(ushort));

			if (stream.Length - initialPosition < size)
				return null;

			var opcode = (ShardClientOpcode)ordinal;

			if (packetMap.ContainsKey(opcode))
			{
				Console.WriteLine($"received packet from client: type = {opcode}, total size = {size}");
				return (IInPacket)Activator.CreateInstance(packetMap[opcode]);
			}
			else
			{
				Console.WriteLine($"received unimplemented packet from client: type = {opcode}, total size = {size}");
				return new UnimplementedPacket(size);
			}
    }
  }
}
