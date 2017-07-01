/* 
 * The Drama project: what you get when a bunch of actors try to host a game.
 * Copyright (C) 2017 Soulson
 *
 * This program is free software: you can redistribute it and/or modify
 * it under the terms of the GNU Affero General Public License as
 * published by the Free Software Foundation, either version 3 of the
 * License, or (at your option) any later version.
 *
 * This program is distributed in the hope that it will be useful,
 * but WITHOUT ANY WARRANTY; without even the implied warranty of
 * MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
 * GNU Affero General Public License for more details.
 *
 * You should have received a copy of the GNU Affero General Public License
 * along with this program.  If not, see <http://www.gnu.org/licenses/>.
 */

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

		public static void WriteNullTerminatedString(this BinaryWriter writer, string value, Encoding encoding)
		{
			if (writer == null)
				throw new ArgumentNullException(nameof(writer));
			if (value == null)
				throw new ArgumentNullException(nameof(value));
			if (encoding == null)
				throw new ArgumentNullException(nameof(encoding));
			
			writer.Write(encoding.GetBytes(value));
			writer.Write((byte)0);
		}
	}
}
