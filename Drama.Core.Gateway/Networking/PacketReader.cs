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

using Drama.Core.Interfaces;
using Drama.Core.Interfaces.Networking;
using System;
using System.Collections.Generic;
using System.IO;

namespace Drama.Core.Gateway.Networking
{
	public abstract class PacketReader
  {
		public IEnumerable<IInPacket> ProcessData(ArraySegment<byte> data)
    {
			using (var buffer = new MemoryStream(data.Count))
			{
				buffer.Write(data.Array, 0, data.Count);

				buffer.Position = 0;
				while (buffer.Position < data.Count)
				{
					var startingPosition = buffer.Position;
					ProcessBeforeRead(buffer);

					buffer.Position = startingPosition;
					var packet = CreatePacket(buffer, out int packetSize);
					
					if (packet == null)
						throw new DramaException($"packet returned by {nameof(CreatePacket)} was null! this may mean that we're receiving non-whole packets");

					buffer.Position = startingPosition + ReadOffset;
					if (!packet.Read(buffer))
						throw new DramaException($"{nameof(packet.Read)} returned false! this may mean that we're receiving non-whole packets");

					// don't pass unimplemented packets back; they're useless
					if (packet is UnimplementedPacket)
						continue;

					yield return packet;

					buffer.Position = startingPosition + packetSize;
				}
			}
    }

    /// <remarks>
    /// Returns a new IPacket object of the proper type to read the next packet on the stream or null if
    /// there is not enough data to determine the type of the appropriate IPacket. If there is no
    /// appropriate IPacket type to handle the packet, then an UnimplementedPacket object with the correct
    /// size is returned.
    /// </remarks>
    protected abstract IInPacket CreatePacket(Stream stream, out int packetSize);

		protected abstract int ReadOffset { get; }

		protected virtual void ProcessBeforeRead(Stream stream) { }
  }
}
