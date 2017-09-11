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

using Drama.Core.Interfaces.Utilities;
using Orleans;
using System.Numerics;
using System.Threading.Tasks;

namespace Drama.Auth.Interfaces.Utilities
{
	public interface IRandomService : IGrainWithIntegerKey
	{
		Task<BigInteger> GetRandomBigInteger(int sizeInBytes);
		Task<byte[]> GetRandomBytes(int size);
		Task<int> GetRandomInt();
		Task<int> GetRandomInt(int exclusiveUpperBound);

		Task<byte> GetRandomRange(Range<byte> range);
		Task<int> GetRandomRange(Range<int> range);
	}
}
