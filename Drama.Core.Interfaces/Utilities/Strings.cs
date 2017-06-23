using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Drama.Core.Interfaces.Utilities
{
	public static class Strings
	{
		public static long KnuthHash(this string input)
		{
			if (input == null)
				throw new ArgumentNullException(nameof(input));

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

		public static string ReadNullTerminatedString(this BinaryReader reader, Encoding encoding)
		{
			if (reader == null)
				throw new ArgumentNullException(nameof(reader));
			if (encoding == null)
				throw new ArgumentNullException(nameof(encoding));

			var bytes = new List<byte>();

			while(true)
			{
				var read = reader.ReadByte();

				if (read == 0)
					break;

				bytes.Add(read);
			}

			return encoding.GetString(bytes.ToArray());
		}
	}
}
