﻿/* 
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

using Orleans.Concurrency;
using System.Numerics;

namespace Drama.Auth.Interfaces.Account
{
	[Immutable]
	public sealed class SrpInitialParameters
	{
		public BigInteger B { get; }
		public byte G { get; }
		public BigInteger N { get; }
		public BigInteger Salt { get; }
		public BigInteger RandomNumber { get; }

		public SrpInitialParameters(BigInteger b, byte g, BigInteger n, BigInteger salt, BigInteger randomNumber)
		{
			B = b;
			G = g;
			N = n;
			Salt = salt;
			RandomNumber = randomNumber;
		}
	}
}
