using System;
using System.Collections.Generic;

namespace Drama.Shard.Interfaces.Objects
{
	public class MovementUpdate : ObjectUpdate
	{
		public MovementUpdate() : base(ObjectUpdateType.Movement)
		{
		}
	}
}
