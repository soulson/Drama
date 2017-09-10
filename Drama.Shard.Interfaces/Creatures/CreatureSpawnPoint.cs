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

using Drama.Core.Interfaces.Numerics;
using Drama.Shard.Interfaces.Utilities;
using Newtonsoft.Json;
using System;

namespace Drama.Shard.Interfaces.Creatures
{
	/// <summary>
	/// Represents a static creature instance.
	/// </summary>
	public class CreatureSpawnPoint : IDefinitionSetEntry
	{
		public int Id { get; set; }
		public int CreatureDefinitionId { get; set; }
		public int MapId { get; set; }
		public Vector3 Position { get; set; }
		public float Orientation { get; set; }
		public TimeSpan RespawnTime { get; set; }
		public float SpawnRadius { get; set; }
		public bool Dead { get; set; }

		[JsonIgnore]
		public long Discriminator => MapId;
	}
}
