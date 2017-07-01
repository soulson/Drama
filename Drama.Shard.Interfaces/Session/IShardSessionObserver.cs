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
using Orleans;

namespace Drama.Shard.Interfaces.Session
{
	/// <summary>
	/// ShardSessionObservers are objects capable of forwarding notifications
	/// from a ShardSession to the network session responsible for getting them
	/// to the session client.
	/// </summary>
	public interface IShardSessionObserver : IGrainObserver
	{
		/// <summary>
		/// Sends a notification to the network session.
		/// </summary>
		/// <param name="packet">Cannot be null</param>
		void ForwardPacket(IOutPacket packet);
	}
}
