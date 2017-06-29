using System;

namespace Drama.Shard.Interfaces.Units
{
	public enum Form : byte
	{
		None = 0x00,
		Cat = 0x01,
		Tree = 0x02,
		Travel = 0x03,
		Aqua = 0x04,
		Bear = 0x05,
		Ambient = 0x06,
		Ghoul = 0x07,
		DireBear = 0x08,
		CreatureBear = 0x0e,
		CreatureCat = 0x0f,
		GhostWolf = 0x10,
		BattleStance = 0x11,
		DefensiveStance = 0x12,
		BerserkerStance = 0x13,
		Shadow = 0x1c,
		Stealth = 0x1e,
		Moonkin = 0x1f,
		SpiritOfRedemption = 0x20,
	}
}
