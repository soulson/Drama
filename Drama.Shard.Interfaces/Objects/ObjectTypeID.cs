﻿using System;

namespace Drama.Shard.Interfaces.Objects
{
	public enum ObjectTypeID : byte
	{
		Object = 0,
		Item = 1,
		Container = 2,
		Unit = 3,
		Player = 4,
		GameObject = 5,
		DynamicObject = 6,
		Corpse = 7,
	}
}
