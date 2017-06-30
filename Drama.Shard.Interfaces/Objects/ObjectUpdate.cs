using Orleans.Concurrency;
using System;

namespace Drama.Shard.Interfaces.Objects
{
	[Immutable]
	public class ObjectUpdate
	{
		public ObjectUpdateType? UpdateType { get; }
		public MovementUpdate MovementUpdate { get; }
		public ValuesUpdate ValuesUpdate { get; }

		public ObjectUpdate(ObjectUpdateType? type, MovementUpdate movementUpdate, ValuesUpdate valuesUpdate)
		{
			UpdateType = type;
			MovementUpdate = movementUpdate;
			ValuesUpdate = valuesUpdate;
		}

		public ObjectUpdate(MovementUpdate movementUpdate, ValuesUpdate valuesUpdate) : this(null, movementUpdate, valuesUpdate)
		{
		}
	}
}
