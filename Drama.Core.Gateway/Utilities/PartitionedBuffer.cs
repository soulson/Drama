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
      freeIndexPool.Push(partition.Offset);
      Interlocked.Increment(ref freeBlocks);
    }
  }
}
