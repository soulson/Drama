using Orleans.Concurrency;
using System;

namespace Drama.Shard.Interfaces.Objects
{
	[Immutable]
	public class ObjectUpdate
	{
		public ObjectID ObjectId { get; }
		public ObjectTypeID TypeId { get; }
		public ObjectUpdateType? UpdateType { get; }
		public ObjectUpdateFlags UpdateFlags { get; }
		public MovementUpdate MovementUpdate { get; }
		public ValuesUpdate ValuesUpdate { get; }

		public ObjectUpdate(ObjectID objectId, ObjectTypeID typeId, ObjectUpdateFlags updateFlags, MovementUpdate movementUpdate, ValuesUpdate valuesUpdate, ObjectUpdateType? type)
		{
			ObjectId = objectId;
			TypeId = typeId;
			UpdateType = type;
			MovementUpdate = movementUpdate;
			ValuesUpdate = valuesUpdate;
		}

		public ObjectUpdate(ObjectID objectId, ObjectTypeID typeId, ObjectUpdateFlags updateFlags, MovementUpdate movementUpdate, ValuesUpdate valuesUpdate)
			: this(objectId, typeId, updateFlags, movementUpdate, valuesUpdate, null)
		{
		}
	}
}
