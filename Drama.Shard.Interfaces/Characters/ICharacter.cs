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

using Drama.Core.Interfaces.Networking;
using Drama.Shard.Interfaces.Units;
using Orleans;
using Orleans.Concurrency;
using System;
using System.Threading.Tasks;

namespace Drama.Shard.Interfaces.Characters
{
	/// <summary>
	/// The Character grain represents a player character.
	/// 
	/// The key for this grain is the ObjectID of the Character.
	/// </summary>
	public interface ICharacter : ICharacter<CharacterEntity>, IGrainWithIntegerKey
	{

	}

	public interface ICharacter<out TEntity> : IUnit<TEntity>, IGrainWithIntegerKey
		where TEntity : CharacterEntity, new()
	{
		/// <summary>
		/// Creates a new Character.
		/// </summary>
		/// <returns>The Entity of the new Character</returns>
		Task<CharacterEntity> Create(string name, string account, string shard, Race race, Class @class, Sex sex, byte skin, byte face, byte hairStyle, byte hairColor, byte facialHair);

		/// <summary>
		/// Logs the character into the world under the specified ShardSession ID.
		/// </summary>
		Task Login(Guid sessionId);

		/// <summary>
		/// Starts the process of logging the character out of the world.
		/// </summary>
		Task Logout();

		/// <summary>
		/// Gets the ID of the ShardSession for which this Character is an agent.
		/// </summary>
		Task<Guid> GetSessionId();

		/// <summary>
		/// Sends a message to the ShardSession for which this Character is an
		/// agent.
		/// 
		/// This method is one-way.
		/// </summary>
		/// <param name="message">Cannot be null</param>
		[OneWay]
		Task Send(IOutPacket message);

		/// <summary>
		/// Returns true if this Character is ingame.
		/// </summary>
		Task<bool> IsOnline();

		/// <summary>
		/// Gets a snapshot of the state of this Character.
		/// </summary>
		Task<CharacterEntity> GetCharacterEntity();
	}
}
