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

using Drama.Shard.Interfaces.Utilities;
using Orleans;
using System.Threading.Tasks;

namespace Drama.Shard.Interfaces.Creatures
{
	/// <summary>
	/// Defines the properties of a class of Creatures.
	/// 
	/// The key for this grain is templateId of this Creature definition, also
	/// known as the creature "entry".
	/// </summary>
	public interface ICreatureTemplate : IGrainWithIntegerKey, IMergeable<CreatureTemplateEntity>
	{
		/// <summary>
		/// Returns true if this CreatureTemplate is defined.
		/// </summary>
		Task<bool> Exists();

		/// <summary>
		/// Gets a CreatureTemplateEntity describing this CreatureTemplate.
		/// </summary>
		Task<CreatureTemplateEntity> GetEntity();
	}
}
