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
