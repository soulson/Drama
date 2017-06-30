using System;

namespace Drama.Shard.Interfaces.Objects
{
	[Flags]
	public enum ObjectUpdateFlags : byte
	{
		None = 0x00,
		Self = 0x01,
		Transport = 0x02,
		FullId = 0x04,
		MaskId = 0x08,
		All = 0x10,
		Living = 0x20,
		HasPosition = 0x40,
	}
}
