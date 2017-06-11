using System;
using System.Text;

namespace Drama.Core.Interfaces.Utilities
{
	public static class Strings
	{
		public static long KnuthHash(this string input)
		{
			long hash = 3074457345618258791;

			unchecked
			{
				foreach (var point in input)
				{
					hash += point;
					hash *= 3074457345618258799;
				}
			}

			return hash;
		}
	}
}
