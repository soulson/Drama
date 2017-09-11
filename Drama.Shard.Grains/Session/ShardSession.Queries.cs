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

using Drama.Shard.Interfaces.Characters;
using Drama.Shard.Interfaces.Creatures;
using Drama.Shard.Interfaces.Protocol;
using System.Threading.Tasks;

namespace Drama.Shard.Grains.Session
{
	public partial class ShardSession
	{
		public async Task<QueryNameResponse> QueryName(QueryNameRequest request)
		{
			VerifyIngame();
			
			var character = GrainFactory.GetGrain<ICharacter>(request.ObjectId);
			var entity = await character.GetCharacterEntity();

			var response = new QueryNameResponse()
			{
				ObjectId = request.ObjectId,
				Class = entity.Class,
				CrossShardId = 0, // NYI
				Name = entity.Name,
				Race = entity.Race,
				Sex = entity.Sex,
			};

			return response;
		}

		public async Task<QueryCreatureResponse> QueryCreature(QueryCreatureRequest request)
		{
			VerifyIngame();

			var creature = await GrainFactory.GetGrain<ICreature>(request.ObjectId).GetCreatureEntity();
			var creatureDefinition = await GrainFactory.GetGrain<ICreatureDefinition>(request.CreatureDefinitionId).GetEntity();

			var response = new QueryCreatureResponse
			{
				Civilian = creatureDefinition.Civilian,
				CreatureDefinitionId = creatureDefinition.Id,
				Family = creatureDefinition.Family,
				Flags = creatureDefinition.CreatureFlags,
				ModelId = creature.DisplayID,
				Name = creatureDefinition.Name,
				PetSpellDataId = creatureDefinition.PetSpellDataId,
				Rank = creatureDefinition.Rank,
				Subname = creatureDefinition.Subname,
				Type = creatureDefinition.Type,
			};

			return response;
		}
	}
}
