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

using System.IO;

namespace Drama.Core.Interfaces.Networking
{
	public interface IInPacket
  {
    /// <remarks>
    /// Reads this packet from the current stream. If there is not enough data on the stream to read the
    /// entire packet, then this method returns false.
    /// </remarks>
    /// <param name="stream">stream from which to read</param>
    /// <returns>true if entire packet was read; false if not</returns>
    bool Read(Stream stream);
  }
}
