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

using Drama.Auth.Interfaces;
using Drama.Auth.Interfaces.Utilities;
using Drama.Shard.Interfaces.Characters;
using Drama.Shard.Interfaces.Objects;
using Drama.Shard.Interfaces.Protocol;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Drama.Shard.Grains.Session
{
	public partial class ShardSession
	{
		public async Task<IList<CharacterEntity>> GetCharacterList()
		{
			VerifyAuthenticated();

			var result = new List<CharacterEntity>();
			var characterList = GrainFactory.GetGrain<ICharacterList>(ShardName);
			var characterIds = await characterList.GetCharactersByAccount(AuthenticatedIdentity);

			foreach(var id in characterIds)
			{
				var character = GrainFactory.GetGrain<ICharacter>(id);
				result.Add(await character.GetEntity());
			}

			return result;
		}

		public async Task CreateCharacter(CharacterCreateRequest request)
		{
			VerifyAuthenticated();

			var characterList = GrainFactory.GetGrain<ICharacterList>(ShardName);
			var id = await characterList.AddCharacter(request.Name, AuthenticatedIdentity);
			var character = GrainFactory.GetGrain<ICharacter>(id);
			var entity = await character.Create(request.Name, AuthenticatedIdentity, ShardName, request.Race, request.Class, request.Sex, request.Skin, request.Face, request.HairStyle, request.HairColor, request.FacialHair);
		}

		public async Task Login(ObjectID characterId)
		{
			VerifyAuthenticated();

			var character = GrainFactory.GetGrain<ICharacter>(characterId);

			if (await character.Exists())
			{
				var entity = await character.GetEntity();

				// sanity checks
				if(entity.Account != AuthenticatedIdentity)
				{
					GetLogger().Warn($"received login request from account {AuthenticatedIdentity} for character {entity.Name}, owned by {entity.Account}");
					return;
				}
				if(entity.Shard != ShardName)
				{
					GetLogger().Warn($"received login request from account {AuthenticatedIdentity} for character {entity.Name} which exists in shard {entity.Shard}, but this shard is {ShardName}");
					return;
				}

				GetLogger().Info($"account {AuthenticatedIdentity} logs in as character {entity.Name}");

				ActiveCharacter = character;
				var sendTasks = new LinkedList<Task>();

				var loginVerifyWorld = new LoginVerifyWorldRequest()
				{
					MapId = entity.MapId,
					Position = entity.Position,
					Orientation = entity.Orientation,
				};
				sendTasks.AddLast(Send(loginVerifyWorld));

				var accountDataTimes = new AccountDataTimesRequest();
				sendTasks.AddLast(Send(accountDataTimes));

				var loginSetRestStart = new LoginSetRestStartRequest();
				sendTasks.AddLast(Send(loginSetRestStart));

				var updateBindPoint = new UpdateBindPointRequest()
				{
					// TODO: implement bind point
					Position = entity.Position,
					MapId = entity.MapId,
					ZoneId = entity.ZoneId,
				};
				sendTasks.AddLast(Send(updateBindPoint));

				var loginInitializeTutorial = new LoginTutorialRequest();
				sendTasks.AddLast(Send(loginInitializeTutorial));

				var loginInitializeSpells = new LoginInitializeSpellsRequest();
				sendTasks.AddLast(Send(loginInitializeSpells));

				var loginInitializeActionButtons = new LoginInitializeActionButtonsRequest();
				sendTasks.AddLast(Send(loginInitializeActionButtons));

				var timeService = GrainFactory.GetGrain<ITimeService>(0);
				var loginSetTimeAndSpeed = new LoginSetTimeAndSpeedRequest()
				{
					// TODO: configurize this?
					GameSpeed = 0.01666667f,
					ServerTime = await timeService.GetNow(),
				};
				sendTasks.AddLast(Send(loginSetTimeAndSpeed));

				var friendList = new FriendListResponse();
				sendTasks.AddLast(Send(friendList));

				var ignoreList = new IgnoreListResponse();
				sendTasks.AddLast(Send(ignoreList));

				var initializeWorldState = new InitializeWorldStateRequest()
				{
					MapId = entity.MapId,
					ZoneId = entity.ZoneId,
				};
				sendTasks.AddLast(Send(initializeWorldState));

				var createUpdate = await ActiveCharacter.GetCreationUpdate();
				var objectUpdateRequest = new ObjectUpdateRequest()
				{
					TargetObjectId = entity.Id,
				};
				objectUpdateRequest.ObjectUpdates.Add(createUpdate);
				sendTasks.AddLast(Send(objectUpdateRequest));

				await Task.WhenAll(sendTasks);
			}
			else
				GetLogger().Warn($"received login request from account {AuthenticatedIdentity} for non-existing character id {characterId}");
		}
	}
}
