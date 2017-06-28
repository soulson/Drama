using System;

namespace Drama.Shard.Interfaces.Objects
{
	public enum ObjectFields : short
	{
		Id = 0x00, // Size:2
		Type = 0x02, // Size:1
		Entry = 0x03, // Size:1
		Scale = 0x04, // Size:1
		Padding = 0x05, // Size:1

		END = 0x06,
	}
}
