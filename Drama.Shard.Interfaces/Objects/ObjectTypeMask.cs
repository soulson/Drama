using System;

namespace Drama.Shard.Interfaces.Objects
{
	[Flags]
	public enum ObjectTypeMask : uint
	{
		None = 0x0000,
		Object = 0x0001,
		Item = 0x0002,
		Container = 0x0004,
		Unit = 0x0008,
		Player = 0x0010,
		GameObject = 0x0020,
		DynamicObject = 0x0040,
		Corpse = 0x0080,
	}
}
