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
	/// <summary>
	/// ValuesUpdate is a type of ObjectUpdate block that updates the Fields
	/// array of a PersistentObject.
	/// </summary>
	[Immutable]
	public class ValuesUpdate
	{
		/// <summary>
		/// The number of 4-byte blocks in the UpdateMask of this ValuesUpdate.
		/// </summary>
		public byte BlockCount { get; }

		/// <summary>
		/// The bit vector representing which PersistentObject fields have changed.
		/// </summary>
		public ImmutableArray<byte> UpdateMask { get; }

		/// <summary>
		/// The values of all fields whose values are being changed by this update.
		/// </summary>
		public ImmutableArray<int> Fields { get; }

		/// <summary>
		/// Creates a new ValuesUpdate.
		/// </summary>
		/// <param name="entity">Cannot be null</param>
		/// <param name="isCreating">
		/// True if this object is being created by this update, false otherwise
		/// </param>
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
