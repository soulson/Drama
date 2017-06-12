using System;

namespace Drama.Auth.Interfaces.Shard
{
	[Flags]
	public enum ShardFlags
	{
		None = 0x00,
		Invalid = 0x01,
		Offline = 0x02,
		SpecifyBuild = 0x04,
		NewPlayers = 0x20,
		Recommended = 0x40,
		Full = 0x80,
	}
}
