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

namespace Drama.Tools.Load.Formats.Sql
{
	/// <summary>
	/// Interface for database-layer entities used in ORM.
	/// </summary>
	/// <typeparam name="TGrainEntity"></typeparam>
	public interface ISqlEntity<TGrainEntity> where TGrainEntity : new()
	{
		/// <summary>
		/// Gets a 64-bit integer primary key for this entity.
		/// </summary>
		long GetKey();

		/// <summary>
		/// Converts this database-layer ORM entity into a Grain-layer entity.
		/// </summary>
		TGrainEntity ToGrainEntity();
	}
}
