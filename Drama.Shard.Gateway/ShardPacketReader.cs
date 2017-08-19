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

using Drama.Core.Gateway.Networking;
using Drama.Core.Interfaces;
using Drama.Core.Interfaces.Networking;
using Drama.Shard.Interfaces.Protocol;
using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.IO;
using System.Linq;
using System.Reflection;

namespace Drama.Shard.Gateway
{
	public class ShardPacketReader : PacketReader
  {
		private const int ClientPacketHeaderSize = 6;

		private readonly ShardPacketCipher packetCipher;
		private readonly ImmutableDictionary<ShardClientOpcode, Type> packetMap;

		protected override int ReadOffset => 6;

		public ShardPacketReader(ShardPacketCipher packetCipher, Assembly packetDefinitionAssembly)
		{
			this.packetCipher = packetCipher;

			var annotatedTypes =
				from type in packetDefinitionAssembly.GetExportedTypes()
				where type.GetTypeInfo().GetCustomAttributes<ClientPacketAttribute>().Any()
				select type;

			packetMap = ImmutableDictionary.CreateRange(
				annotatedTypes.SelectMany(
					annotatedType => annotatedType.GetTypeInfo().GetCustomAttributes<ClientPacketAttribute>().Select(
						attribute => new KeyValuePair<ShardClientOpcode, Type>(attribute.Opcode, annotatedType)
					)
				)
			);
		}

    protected override IInPacket CreatePacket(Stream stream, out int packetSize)
    {
			packetSize = 0;
			if (stream.Length - stream.Position < ClientPacketHeaderSize)
				return null;

			var initialPosition = stream.Position;
			var header = new byte[ClientPacketHeaderSize];
			stream.Read(header, 0, header.Length);

			// the size is encoded in big endian and doesn't include the size of the size itself
			var size = sizeof(ushort) + BitConverter.ToUInt16(new[] { header[1], header[0] }, 0);
			var ordinal = BitConverter.ToUInt32(header, sizeof(ushort));

			packetSize = size;

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
				Console.WriteLine($"received unimplemented packet from client: type = {opcode} (0x{(ushort)opcode:x4}), total size = {size}");
				return new UnimplementedPacket(size - ClientPacketHeaderSize);
			}
    }

		protected override void ProcessBeforeRead(Stream stream)
		{
			var startingPosition = stream.Position;
			if (stream.Length - stream.Position < ClientPacketHeaderSize)
				throw new DramaException("packet size is too small to decrypt");

			var header = new byte[ClientPacketHeaderSize];
			stream.Read(header, 0, header.Length);

			packetCipher.DecryptHeader(new ArraySegment<byte>(header));

			stream.Position = startingPosition;
			stream.Write(header, 0, header.Length);
		}
	}
}
