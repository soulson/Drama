using Orleans.Concurrency;
using System;
using System.Collections.Immutable;

namespace Drama.Shard.Interfaces.Objects
{
	[Immutable]
	public class ValuesUpdate
	{
		public ObjectID ObjectId { get; }

		public byte BlockCount { get; }
		public ImmutableArray<byte> UpdateMask { get; }
		public ImmutableArray<int> Fields { get; }

		public ValuesUpdate(ObjectEntity entity, bool isCreating)
		{
			if (entity == null)
				throw new ArgumentNullException(nameof(entity));

			var mask = isCreating ? entity.CreateMask : entity.UpdateMask;

			ObjectId = entity.Id;
			BlockCount = mask.BlockCount;
			UpdateMask = mask.Data.ToImmutableArray();

			var fieldBuilder = ImmutableArray.CreateBuilder<int>(mask.ActiveBitCount);
			for(int i = 0; i < mask.ValueCount; ++i)
			{
				if (mask.GetBit(i))
					fieldBuilder.Add(entity.Fields[i]);
			}
			Fields = fieldBuilder.ToImmutable();
		}
	}
}
