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

using System;
using System.Collections.Generic;
using System.Threading;

namespace Drama.Core.Gateway.Utilities
{
	public class PartitionedBuffer
  {
    private readonly byte[] buffer;
    private readonly Stack<int> freeIndexPool;

    private int nextIndex;
    private int freeBlocks;

    public int BlockCount { get; }
    public int BlockSize { get; }
    public int LeasedBlocks => BlockCount - freeBlocks;

    public PartitionedBuffer(int blockSize, int blockCount)
    {
      if (blockSize <= 0)
        throw new ArgumentException($"{nameof(blockSize)} must be greater than 0");
      if (blockCount <= 0)
        throw new ArgumentException($"{nameof(blockCount)} must be greater than 0");

      BlockSize = blockSize;
      BlockCount = blockCount;

      buffer = new byte[BlockSize * BlockCount];
      freeIndexPool = new Stack<int>();
      nextIndex = 0;
      freeBlocks = BlockCount;
    }

    public bool TryCheckOutPartition(out ArraySegment<byte> partition)
    {
      if (freeIndexPool.Count > 0)
      {
        partition = new ArraySegment<byte>(buffer, freeIndexPool.Pop(), BlockSize);
        Interlocked.Decrement(ref freeBlocks);
        return true;
      }
      else
      {
        if (nextIndex + BlockSize > buffer.Length)
        {
          partition = default(ArraySegment<byte>);
          return false;
        }
        else
        {
          partition = new ArraySegment<byte>(buffer, nextIndex, BlockSize);
          nextIndex += BlockSize;
          Interlocked.Decrement(ref freeBlocks);
          return true;
        }
      }
    }

    public void CheckInPartition(ArraySegment<byte> partition)
    {
      if (freeIndexPool.Contains(partition.Offset))
        throw new InvalidOperationException($"the partition at 0x{partition.Offset:x} is already checked in");

      freeIndexPool.Push(partition.Offset);
      Interlocked.Increment(ref freeBlocks);
    }
  }
}
