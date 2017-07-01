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

using Orleans.Concurrency;
using System;
using System.Collections.Immutable;

namespace Drama.Shard.Interfaces.Objects
{
	[Immutable]
	public class ValuesUpdate
	{
		public byte BlockCount { get; }
		public ImmutableArray<byte> UpdateMask { get; }
		public ImmutableArray<int> Fields { get; }

		public ValuesUpdate(ObjectEntity entity, bool isCreating)
		{
			if (entity == null)
				throw new ArgumentNullException(nameof(entity));

			var mask = isCreating ? entity.CreateMask : entity.UpdateMask;
			
			BlockCount = mask.BlockCount;
			UpdateMask = mask.Data.ToImmutableArray();

			var fieldBuilder = ImmutableArray.CreateBuilder<int>(mask.ActiveBitCount);
			for(short i = 0; i < mask.ValueCount; ++i)
			{
				if (mask.GetBit(i))
					fieldBuilder.Add(entity.GetFieldSigned(i));
			}
			Fields = fieldBuilder.ToImmutable();
		}
	}
}
