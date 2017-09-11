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
using Drama.Shard.Interfaces.Creatures;
using System.IO;
using System.Text;

namespace Drama.Shard.Interfaces.Protocol
{
	public sealed class QueryCreatureResponse : AbstractOutPacket
	{
		public override ShardServerOpcode Opcode => ShardServerOpcode.QueryCreature;

		public int CreatureDefinitionId { get; set; }
		public string Name { get; set; }
		public string Subname { get; set; }
		public CreatureFlags Flags { get; set; }
		public CreatureType Type { get; set; }
		public CreatureFamily Family { get; set; }
		public CreatureRank Rank { get; set; }
		public int PetSpellDataId { get; set; }
		public int ModelId { get; set; }
		public bool Civilian { get; set; }

		protected override void Write(BinaryWriter writer)
		{
			writer.Write(CreatureDefinitionId);

			if (Name == null)
				writer.Write((byte)0);
			else
				writer.WriteNullTerminatedString(Name, Encoding.UTF8);

			// these are apparently for unused name strings
			writer.Write((byte)0);
			writer.Write((byte)0);
			writer.Write((byte)0);

			if (Subname == null)
				writer.Write((byte)0);
			else
				writer.WriteNullTerminatedString(Subname, Encoding.UTF8);

			writer.Write((int)Flags);
			writer.Write((int)Type);
			writer.Write((int)Family);
			writer.Write((int)Rank);

			writer.Write(0);

			writer.Write(PetSpellDataId);
			writer.Write(ModelId);

			if (Civilian)
				writer.Write((short)1);
			else
				writer.Write((short)0);
		}
	}
}
