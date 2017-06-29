using System;

namespace Drama.Shard.Interfaces.Units
{
	[Flags]
	public enum RaceFlags : int
	{
		None = 0x00,
		NotPlayable = 0x01,
		BareFeet = 0x02,
		CanMount = 0x04,
	}
}
