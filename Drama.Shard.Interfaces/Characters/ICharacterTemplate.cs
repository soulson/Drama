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

using Drama.Shard.Interfaces.Utilities;
using Orleans;
using System.Threading.Tasks;

namespace Drama.Shard.Interfaces.Characters
{
	/// <summary>
	/// Defines initial values for a Character by Race and Class.
	/// 
	/// The key for this grain is (Race &lt;&lt; 8) + Class
	/// </summary>
	public interface ICharacterTemplate : IGrainWithIntegerKey, IMergeable<CharacterTemplateEntity>
	{
		/// <summary>
		/// Returns true if this CharacterTemplate is defined.
		/// </summary>
		Task<bool> Exists();

		/// <summary>
		/// Gets a CharacterTemplateEntity describing this map.
		/// </summary>
		Task<CharacterTemplateEntity> GetEntity();
	}
}