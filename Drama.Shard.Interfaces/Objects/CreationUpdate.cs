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

using Orleans.Concurrency;
using System;

namespace Drama.Shard.Interfaces.Objects
{
	[Immutable]
	public class CreationUpdate : ObjectUpdate
	{
		public CreationUpdate(ObjectID id, ObjectTypeID typeId, ObjectUpdateFlags updateFlags, MovementUpdate movementUpdate, ValuesUpdate valuesUpdate) : base(
			id,
			typeId,
			updateFlags,
			movementUpdate ?? throw new ArgumentNullException(nameof(movementUpdate)),
			valuesUpdate ?? throw new ArgumentNullException(nameof(valuesUpdate)),
			id.ObjectType == ObjectID.Type.Corpse ||
			id.ObjectType == ObjectID.Type.DynamicObject ||
			id.ObjectType == ObjectID.Type.GameObject ||
			id.ObjectType == ObjectID.Type.Player ||
			id.ObjectType == ObjectID.Type.Unit ?
			ObjectUpdateType.CreateObject2 :
			ObjectUpdateType.CreateObject)
		{
		}
	}
}
