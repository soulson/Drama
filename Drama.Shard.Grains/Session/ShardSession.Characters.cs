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

				var loginVerifyWorld = new LoginVerifyWorldRequest()
				{
					MapId = entity.MapId,
					Position = entity.Position,
					Orientation = entity.Orientation,
				};
				var sendLoginVerifyWorld = Send(loginVerifyWorld);

				var accountDataTimes = new AccountDataTimesRequest();
				var sendAccountDataTimes = Send(accountDataTimes);

				var loginSetRestStart = new LoginSetRestStartRequest();
				var sendLoginSetRestStart = Send(loginSetRestStart);

				var updateBindPoint = new UpdateBindPointRequest()
				{
					// TODO: implement bind point
					Position = entity.Position,
					MapId = entity.MapId,
					ZoneId = entity.ZoneId,
				};
				var sendUpdateBindPoint = Send(updateBindPoint);

				var loginInitializeTutorial = new LoginTutorialRequest();
				var sendLoginInitializeTutorial = Send(loginInitializeTutorial);

				var loginInitializeSpells = new LoginInitializeSpellsRequest();
				var sendLoginInitializeSpells = Send(loginInitializeSpells);

				var loginInitializeActionButtons = new LoginInitializeActionButtonsRequest();
				var sendLoginInitializeActionButtons = Send(loginInitializeActionButtons);

				var timeService = GrainFactory.GetGrain<ITimeService>(0);
				var loginSetTimeAndSpeed = new LoginSetTimeAndSpeedRequest()
				{
					// TODO: configurize this?
					GameSpeed = 0.01666667f,
					ServerTime = await timeService.GetNow(),
				};
				var sendLoginSetTimeAndSpeed = Send(loginSetTimeAndSpeed);

				var friendList = new FriendListResponse();
				var sendFriendList = Send(friendList);

				var ignoreList = new IgnoreListResponse();
				var sendIgnoreList = Send(ignoreList);

				var initializeWorldState = new InitializeWorldStateRequest()
				{
					MapId = entity.MapId,
					ZoneId = entity.ZoneId,
				};
				var sendInitializeWorldState = Send(initializeWorldState);

				var createUpdate = await ActiveCharacter.GetCreationUpdate();
				var objectUpdateRequest = new ObjectUpdateRequest()
				{
					TargetObjectId = entity.Id,
				};
				objectUpdateRequest.ObjectUpdates.Add(createUpdate);
				var sendObjectUpdate = Send(objectUpdateRequest);

				await Task.WhenAll(sendLoginVerifyWorld, sendAccountDataTimes, sendLoginSetRestStart, sendUpdateBindPoint, sendLoginInitializeTutorial, sendLoginInitializeSpells, sendLoginInitializeActionButtons, sendLoginSetTimeAndSpeed, sendFriendList, sendIgnoreList, sendInitializeWorldState, sendObjectUpdate);
			}
			else
				GetLogger().Warn($"received login request from account {AuthenticatedIdentity} for non-existing character id {characterId}");
		}
	}
}
