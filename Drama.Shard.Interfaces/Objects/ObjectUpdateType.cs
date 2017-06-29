using System;

namespace Drama.Shard.Interfaces.Objects
{
	public enum ObjectUpdateType : byte
	{
		Values = 0,
		Movement = 1,
		CreateObject = 2,
		CreateObject2 = 3,
		OutOfRangeObjects = 4,
		NearObjects = 5,
	}
}
