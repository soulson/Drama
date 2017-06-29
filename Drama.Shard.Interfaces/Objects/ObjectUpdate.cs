using System;

namespace Drama.Shard.Interfaces.Objects
{
	public abstract class ObjectUpdate
	{
		public ObjectUpdateType UpdateType { get; }

		public ObjectUpdate(ObjectUpdateType type)
		{
			UpdateType = type;
		}
	}
}
