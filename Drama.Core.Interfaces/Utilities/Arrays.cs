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

namespace Drama.Core.Interfaces.Utilities
{
	public static class Arrays
	{
		public static T[] Right<T>(T[] input, int length)
		{
			if (input.Length == length)
				return input;

			T[] output = new T[length];
			if (input.Length > length)
				Array.Copy(input, input.Length - length, output, 0, length);
			else
				Array.Copy(input, 0, output, length - input.Length, input.Length);

			return output;
		}

		public static T[] Left<T>(T[] input, int length)
		{
			if (input.Length == length)
				return input;

			T[] output = new T[length];
			if (input.Length > length)
				Array.Copy(input, 0, output, 0, length);
			else
				Array.Copy(input, 0, output, 0, input.Length);

			return output;
		}

		public static T[] Reverse<T>(T[] input)
		{
			T[] output = new T[input.Length];
			Array.Copy(input, output, input.Length);
			Array.Reverse(output);
			return output;
		}

		public static bool AreEqual<T>(T[] a, T[] b)
		{
			if (a.Length != b.Length)
				return false;

			for (int i = 0; i < a.Length; ++i)
			{
				if (!Equals(a[i], b[i]))
					return false;
			}

			return true;
		}
	}
}
