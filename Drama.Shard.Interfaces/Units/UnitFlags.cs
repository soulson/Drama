using System;

namespace Drama.Shard.Interfaces.Units
{
	/// <remarks>
	/// UnitFlags.Flags
	/// </remarks>
	[Flags]
	public enum UnitFlags : uint
	{
		None = 0x00000000,
		NotAttackable = 0x00000002,
		DisableMovement = 0x00000004,
		PvPUnit = 0x00000008,
		Rename = 0x00000010,
		Resting = 0x00000020,
		NotAttackableWhenOutOfCombat = 0x00000100,
		CannotAttack = 0x00000200,
		PvPFlagged = 0x00001000,
		Pacified = 0x00020000,
		DisableRotation = 0x00040000,
		InCombat = 0x00080000,
		NotSelectable = 0x02000000,
		Skinnable = 0x04000000,
		DetectMagic = 0x08000000,
		Sheath = 0x40000000,
	}

	/// <remarks>
	/// UnitFlags.NpcFlags
	/// </remarks>
	[Flags]
	public enum NpcFlags : uint
	{
		None = 0x00000000,
		Gossip = 0x00000001,
		QuestGiver = 0x00000002,
		Vendor = 0x00000004,
		FlightMaster = 0x00000008,
		Trainer = 0x00000010,
		SpiritHealer = 0x00000020,
		SpiritGuide = 0x00000040,
		Innkeeper = 0x00000080,
		Banker = 0x00000100,
		Petitioner = 0x00000200,
		TabardDesigner = 0x00000400,
		Battlemaster = 0x00000800,
		Auctioneer = 0x00001000,
		StableMaster = 0x00002000,
		Repairman = 0x00004000,
	}

	/// <remarks>
	/// UnitFlags.Bytes1[3]
	/// </remarks>
	[Flags]
	public enum UnitFlags1 : byte
	{
		AlwaysStand = 0x01,
		Creep = 0x02,
		Untrackable = 0x04,
	}

	/// <remarks>
	/// UnitFlags.Bytes2[1]
	/// </remarks>
	[Flags]
	public enum UnitFlags2 : byte
	{
		Supportable = 0x08,
		CanHaveAuras = 0x10,
	}
}
