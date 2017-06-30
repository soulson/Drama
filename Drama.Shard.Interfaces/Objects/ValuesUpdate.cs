using Orleans.Concurrency;
using System;
using System.Collections.Immutable;

namespace Drama.Shard.Interfaces.Objects
{
	[Immutable]
	public class ValuesUpdate
	{
		public byte BlockCount { get; }
		public ImmutableArray<byte> UpdateMask { get; }
		public ImmutableArray<int> Fields { get; }

		public ValuesUpdate(ObjectEntity entity, bool isCreating)
		{
			if (entity == null)
				throw new ArgumentNullException(nameof(entity));

			var mask = isCreating ? entity.CreateMask : entity.UpdateMask;
			
			BlockCount = mask.BlockCount;
			UpdateMask = mask.Data.ToImmutableArray();

			var fieldBuilder = ImmutableArray.CreateBuilder<int>(mask.ActiveBitCount);
			for(short i = 0; i < mask.ValueCount; ++i)
			{
				if (mask.GetBit(i))
					fieldBuilder.Add(entity.GetFieldSigned(i));
			}
			Fields = fieldBuilder.ToImmutable();
		}
	}
}
