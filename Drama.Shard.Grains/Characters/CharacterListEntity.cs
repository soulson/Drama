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

using Drama.Shard.Interfaces.Objects;
using System.Collections.Generic;

namespace Drama.Shard.Grains.Characters
{
	public class CharacterListEntity
	{
		// important to start this at 1. a character with id 0 will crash the client on load
		public long NextId { get; set; } = 1L;
		public SortedDictionary<string, ObjectID> CharacterByName { get; } = new SortedDictionary<string, ObjectID>();
		public SortedDictionary<string, IList<ObjectID>> CharactersByAccount { get; } = new SortedDictionary<string, IList<ObjectID>>();
	}
}
