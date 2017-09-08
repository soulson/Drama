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
using Drama.Shard.Interfaces.Spells;
using Drama.Shard.Interfaces.Units;
using Drama.Shard.Interfaces.Utilities;
using System.Collections.Generic;

namespace Drama.Shard.Interfaces.Creatures
{
	/// <summary>
	/// Persisted storage for CreatureDefinition grains.
	/// </summary>
	public class CreatureDefinitionEntity : AbstractDefinitionEntity
	{
		public int Id { get; set; }
		public ISet<int> ModelIds { get; } = new SortedSet<int>();
		public string Name { get; set; }
		public string Subname { get; set; }
		public Range<byte> Level { get; set; }
		public Range<int> Health { get; set; }
		public Range<int> Mana { get; set; }
		public int Armor { get; set; }
		public NpcFlags NpcFlags { get; set; }
		public float SpeedWalking { get; set; }
		public float SpeedRunning { get; set; }
		public float Scale { get; set; }
		public Range<int> AttackDamage { get; set; }
		public Range<int> AttackDamageRanged { get; set; }
		public School AttackSchool { get; set; }
		public int AttackPower { get; set; }
		public int AttackPowerRanged { get; set; }
		public int AttackTimeMilliseconds { get; set; }
		public int AttackTimeRangedMilliseconds { get; set; }
		public float DamageMultiplier { get; set; }
		public Class Class { get; set; }
		public UnitFlags UnitFlags { get; set; }
	}
}
