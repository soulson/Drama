using Orleans.Concurrency;
using System;

namespace Drama.Shard.Interfaces.Objects
{
	[Immutable]
	public class CreationUpdate : ObjectUpdate
	{
		public CreationUpdate(ObjectID id, ObjectTypeID typeId, ObjectUpdateFlags updateFlags, MovementUpdate movementUpdate, ValuesUpdate valuesUpdate) : base(
			id,
			typeId,
			updateFlags,
			movementUpdate ?? throw new ArgumentNullException(nameof(movementUpdate)),
			valuesUpdate ?? throw new ArgumentNullException(nameof(valuesUpdate)),
			id.ObjectType == ObjectID.Type.Corpse ||
			id.ObjectType == ObjectID.Type.DynamicObject ||
			id.ObjectType == ObjectID.Type.GameObject ||
			id.ObjectType == ObjectID.Type.Player ||
			id.ObjectType == ObjectID.Type.Unit ?
			ObjectUpdateType.CreateObject2 :
			ObjectUpdateType.CreateObject)
		{
		}
	}
}
