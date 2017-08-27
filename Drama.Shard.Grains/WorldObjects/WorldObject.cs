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
using Drama.Shard.Grains.Objects;
using Drama.Shard.Interfaces.Objects;
using Drama.Shard.Interfaces.WorldObjects;
using System.Threading.Tasks;

namespace Drama.Shard.Grains.WorldObjects
{
	public sealed class WorldObject : AbstractWorldObject<WorldObjectEntity>, IWorldObject
	{

	}

	public abstract class AbstractWorldObject<TEntity> : AbstractObject<TEntity>, IWorldObject<TEntity>
		where TEntity : WorldObjectEntity, new()
	{
		public Task SetPosition(Vector3 position, float orientation)
		{
			VerifyExists();

			State.Position = position;
			State.Orientation = orientation;

			IsMovementUpdated = true;

			return WriteStateAsync();
		}

		public virtual Task Destroy()
		{
			VerifyExists();

			NotifySubscribers(observer => observer.HandleObjectDestroyed(State));
			return Task.CompletedTask;
		}

		public virtual Task<bool> IsIngame()
			=> Task.FromResult(true);

		protected override MovementUpdate BuildMovementUpdate()
			=> new MovementUpdate(State);
	}
}
