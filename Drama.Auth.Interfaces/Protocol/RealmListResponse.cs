using Drama.Auth.Interfaces.Shard;
using Drama.Core.Interfaces.Networking;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Drama.Auth.Interfaces.Protocol
{
	public sealed class RealmListResponse : IOutPacket
	{
		public IList<ShardEntity> ShardList { get; } = new List<ShardEntity>();

		public void Write(Stream stream)
		{
			using (var writer = new BinaryWriter(stream, Encoding.UTF8, true))
			{
				writer.Write((byte)AuthRequestOpcode.RealmList);

				var sizePosition = stream.Position;
				writer.Write((ushort)7); // number of remaining bytes

				var sizeStartPosition = stream.Position;
				writer.Write(0);
				writer.Write((byte)Math.Min(ShardList.Count, byte.MaxValue)); // number of shards in list

				for (int i = 0; i < Math.Min(ShardList.Count, byte.MaxValue); ++i)
				{
					var shard = ShardList[i];

					writer.Write((int)shard.ShardType);
					writer.Write((byte)shard.ShardFlags);
					writer.Write(Encoding.UTF8.GetBytes(shard.Name));
					writer.Write((byte)0); // null terminator
					writer.Write(Encoding.UTF8.GetBytes($"{shard.Address}:{shard.Port}"));
					writer.Write((byte)0); // null terminator
					writer.Write(0.0f); // "population"
					writer.Write((byte)0); // TODO: character count
					writer.Write((byte)0); // "category";
					writer.Write((byte)0); // ?
				}

				writer.Write((short)2); // ?

				var finalPosition = stream.Position;

				stream.Position = sizePosition;
				writer.Write((ushort)(finalPosition - sizeStartPosition));
				stream.Position = finalPosition;
			}
		}
	}
}
