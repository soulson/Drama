using System;

namespace Drama.Shard.Interfaces.Units
{
	/// <summary>
	/// Enumerates the possible stand states for a unit (standing, sitting, kneeling, etc.)
	/// </summary>
	/// <remarks>
	/// UnitFields.Bytes1[0]
	/// </remarks>
	public enum StandState : byte
	{
		Stand = 0,
		Sit = 1,
		SitChair = 2,
		Sleep = 3,
		SitLowChair = 4,
		SitMediumChair = 5,
		SitHighChair = 6,
		Dead = 7,
		Kneel = 8,
	}
}
