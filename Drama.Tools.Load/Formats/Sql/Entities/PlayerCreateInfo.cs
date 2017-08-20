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
using Drama.Shard.Interfaces.Characters;
using Drama.Shard.Interfaces.Units;
using System.ComponentModel.DataAnnotations.Schema;

namespace Drama.Tools.Load.Formats.Sql.Entities
{
	[Table("playercreateinfo")]
	public class PlayerCreateInfo : ISqlEntity<CharacterTemplateEntity>
	{
		[Column("race")]
		public byte Race { get; set; }
		[Column("class")]
		public byte Class { get; set; }
		[Column("map")]
		public int MapId { get; set; }
		[Column("zone")]
		public int ZoneId { get; set; }
		[Column("position_x")]
		public float PositionX { get; set; }
		[Column("position_y")]
		public float PositionY { get; set; }
		[Column("position_z")]
		public float PositionZ { get; set; }
		[Column("orientation")]
		public float Orientation { get; set; }

		public long GetKey()
			=> (Race << 8) + Class;

		public CharacterTemplateEntity ToGrainEntity()
		{
			return new CharacterTemplateEntity()
			{
				Race = (Race)Race,
				Class = (Class)Class,
				MapId = MapId,
				ZoneId = ZoneId,
				Position = new Vector3(PositionX, PositionY, PositionZ),
				Orientation = Orientation,
			};
		}
	}
}
