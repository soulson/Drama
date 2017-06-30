using System;

namespace Drama.Core.Interfaces.Utilities
{
	public static class DateTimes
	{
		public static int GetBitfield(DateTime time)
			=> unchecked((time.Year - 100) << 24 | time.Month << 20 | (time.Day - 1) << 14 | ((byte)time.DayOfWeek) << 11 | time.Hour << 6 | time.Minute);
	}
}
