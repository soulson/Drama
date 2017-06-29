using System;
using System.Collections.Generic;

namespace Drama.Shard.Interfaces.Objects
{
	public class ValuesUpdate : ObjectUpdate
	{
		public ValuesUpdate() : base(ObjectUpdateType.Values)
		{
		}
	}
}
