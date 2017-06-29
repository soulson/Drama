using System;
using System.Collections.Generic;

namespace Drama.Shard.Interfaces.Objects
{
	public class CreationUpdate : ObjectUpdate
	{
		public CreationUpdate(ObjectID id) : base(
			id.ObjectType == ObjectID.Type.Corpse ||
			id.ObjectType == ObjectID.Type.DynamicObject ||
			id.ObjectType == ObjectID.Type.GameObject ||
			id.ObjectType == ObjectID.Type.Player ||
			id.ObjectType == ObjectID.Type.Unit ?
			ObjectUpdateType.CreateObject2 :
			ObjectUpdateType.CreateObject
		)
		{
		}
	}
}
