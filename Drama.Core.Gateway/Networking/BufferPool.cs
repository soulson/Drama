﻿/* 
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

using Drama.Core.Gateway.Utilities;
using System;
using System.Net.Sockets;

namespace Drama.Core.Gateway.Networking
{
	public class BufferPool
  {
    private readonly PartitionedBuffer buffer;

    public int BlockSize => buffer.BlockSize;
    public int BlockCount => buffer.BlockCount;
    public int LeasedBlocks => buffer.LeasedBlocks;

    public BufferPool(int blockSize, int blockCount)
    {
      buffer = new PartitionedBuffer(blockSize, blockCount);
    }

    public bool TryAssignBuffer(SocketAsyncEventArgs args)
    {
      if (buffer.TryCheckOutPartition(out var partition))
      {
        args.SetBuffer(partition.Array, partition.Offset, partition.Count);
        return true;
      }
      else
        return false;
    }

    public void FreeBuffer(SocketAsyncEventArgs args)
    {
      buffer.CheckInPartition(new ArraySegment<byte>(args.Buffer, args.Offset, args.Count));
      args.SetBuffer(null, 0, 0);
    }
  }
}
