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
using Drama.Shard.Interfaces.Creatures;
using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace Drama.Tools.Load.Formats.Sql.Entities
{
	[Table("creature")]
	public class Creature : ISqlSetEntry<CreatureSpawnPoint>
	{
		[Column("guid")]
		public int Id { get; set; }
		[Column("id")]
		public int CreatureDefinitionId { get; set; }
		[Column("map")]
		public int MapId { get; set; }
		[Column("modelid")]
		public int ModelId { get; set; }
		[Column("equipment_id")]
		public int EquipmentId { get; set; }

		[Column("position_x")]
		public float PositionX { get; set; }
		[Column("position_y")]
		public float PositionY { get; set; }
		[Column("position_z")]
		public float PositionZ { get; set; }

		[Column("orientation")]
		public float Orientation { get; set; }
		[Column("spawntimesecs")]
		public int SpawnTimeSeconds { get; set; }
		[Column("spawndist")]
		public float SpawnRange { get; set; }
		[Column("curhealth")]
		public int CurrentHealth { get; set; }
		[Column("curmana")]
		public int CurrentMana { get; set; }
		[Column("DeathState")]
		public byte DeathStateCode { get; set; }

		public long GetDiscriminator()
			=> MapId;

		public CreatureSpawnPoint ToGrainEntity()
		{
			return new CreatureSpawnPoint
			{
				CreatureDefinitionId = CreatureDefinitionId,
				Dead = DeathStateCode != 0,
				Id = Id,
				MapId = MapId,
				Orientation = Orientation,
				Position = new Vector3(PositionX, PositionY, PositionZ),
				RespawnTime = TimeSpan.FromSeconds(SpawnTimeSeconds),
				SpawnRadius = SpawnRange,
			};
		}
	}
}
