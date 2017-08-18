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

using Orleans;

namespace Drama.Shard.Interfaces.Objects
{
	/// <summary>
	/// ObjectObservers are classes that can be notified when the state of a
	/// PersistentObject is changed.
	/// </summary>
	public interface IObjectObserver : IGrainObserver
	{
		/// <summary>
		/// This method is called when a PersistentObject is becoming
		/// client-visible.
		/// </summary>
		/// <param name="objectEntity">the state of the object that is created</param>
		/// <param name="update">a CreationUpdate describing the object</param>
		void HandleObjectCreate(ObjectEntity objectEntity, CreationUpdate update);

		/// <summary>
		/// This method is called when a PersistentObject's client-visible state
		/// has changed.
		/// </summary>
		/// <param name="objectEntity">the state of the object that has updated</param>
		/// <param name="update">an ObjectUpdate describing what has changed</param>
		void HandleObjectUpdate(ObjectEntity objectEntity, ObjectUpdate update);

		/// <summary>
		/// This method is called when a PersistentObject is destroyed.
		/// </summary>
		/// <param name="objectId">the state of the destroyed object</param>
		void HandleObjectDestroyed(ObjectEntity objectEntity);
	}
}
