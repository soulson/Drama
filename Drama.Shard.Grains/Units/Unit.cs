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
using Drama.Shard.Grains.Objects;
using Drama.Shard.Interfaces.Objects;
using Drama.Shard.Interfaces.Units;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Drama.Shard.Grains.Units
{
	public sealed class Unit : AbstractUnit<UnitEntity>, IUnit
	{

	}

	public abstract class AbstractUnit<TEntity> : AbstractPersistentObject<TEntity>, IUnit<TEntity>, IObjectObserver
		where TEntity : UnitEntity, new()
	{
		// TODO: make this a configuration item
		// TODO: implement a set method that resets the working set timer
		/// <summary>
		/// Characters will update which PersistentObjects are in its working set
		/// with the frequency established by this value.
		/// </summary>
		protected double WorkingSetUpdatePeriodSeconds { get; set; } = 1.0;

		// TODO: make this a configuration item
		/// <summary>
		/// This Unit will "see" other PersistentObjects up to this distance away
		/// with its working set.
		/// </summary>
		protected float ViewDistance { get; set; } = 60.0f;

		private IDisposable workingSetUpdateTimerHandle;
		private readonly ISet<ObjectID> workingSet = new HashSet<ObjectID>();

		public override Task OnDeactivateAsync()
		{
			workingSetUpdateTimerHandle?.Dispose();
			workingSetUpdateTimerHandle = null;

			workingSet.Clear();

			return base.OnDeactivateAsync();
		}

		protected override ObjectUpdateFlags UpdateFlags => base.UpdateFlags | ObjectUpdateFlags.Living;

		protected override MovementUpdate BuildMovementUpdate()
			=> new MovementUpdate(State);

		protected override void OnPostUpdate()
		{
			base.OnPostUpdate();

			State.PreviousMoveFlags = State.MoveFlags;
			State.PreviousOrientation = State.Orientation;
		}

		public Task SetMovementState(MovementFlags movementFlags, int time, int fallTime, Jump jump)
		{
			VerifyExists();

			State.MoveFlags = movementFlags;
			State.MoveTime = time;
			State.FallTime = fallTime;

			if (jump != null)
			{
				State.Jump.Velocity = jump.Velocity;
				State.Jump.SineAngle = jump.SineAngle;
				State.Jump.CosineAngle = jump.CosineAngle;
				State.Jump.XYSpeed = jump.XYSpeed;
			}
			else
				State.Jump.Velocity = State.Jump.SineAngle = State.Jump.CosineAngle = State.Jump.XYSpeed = 0.0f;

			IsMovementUpdated = true;

			return WriteStateAsync();
		}

		protected Task ActivateWorkingSet()
		{
			if (workingSetUpdateTimerHandle == null)
				workingSetUpdateTimerHandle = RegisterTimer(UpdateWorkingSet, null, TimeSpan.FromSeconds(WorkingSetUpdatePeriodSeconds), TimeSpan.FromSeconds(WorkingSetUpdatePeriodSeconds));
			else
				GetLogger().Warn($"{nameof(ActivateWorkingSet)} called but {nameof(workingSetUpdateTimerHandle)} was not null");

			return Task.CompletedTask;
		}

		protected Task DeactivateWorkingSet()
		{
			workingSetUpdateTimerHandle?.Dispose();
			workingSetUpdateTimerHandle = null;
			return ClearWorkingSet();
		}

		private async Task ClearWorkingSet()
		{
			var tasks = new LinkedList<Task>();
			var objectService = GrainFactory.GetGrain<IObjectService>(0);

			foreach (var objectId in workingSet)
			{
				var grainReference = await objectService.GetObject(objectId);
				tasks.AddLast(grainReference.Unsubscribe(this));

				GetLogger().Debug($"{nameof(Unit)} {State.Id} unsubbed from {objectId}");
			}

			workingSet.Clear();

			await Task.WhenAll(tasks);
		}

		/// <remarks>
		/// This method gets invoked by the update timer every
		/// WorkingSetUpdatePeriodSeconds. It updates the working set of this
		/// Character; that is, which PersistentObjects it can see. The argument is
		/// always null.
		/// </remarks>
		private async Task UpdateWorkingSet(object arg)
		{
			var map = await GetMapInstance();
			var newWorkingSet = await map.GetNearbyObjects(State, ViewDistance);

			var addedObjects = newWorkingSet.Except(workingSet);
			var removedObjects = workingSet.Except(newWorkingSet);

			var tasks = new LinkedList<Task>();
			var objectService = GrainFactory.GetGrain<IObjectService>(0);

			foreach (var removedObject in removedObjects)
			{
				var grainReference = await objectService.GetObject(removedObject);
				tasks.AddLast(grainReference.Unsubscribe(this));

				GetLogger().Debug($"{nameof(Unit)} {State.Id} unsubbed from {removedObject}");
			}

			foreach (var addedObject in addedObjects)
			{
				var grainReference = await objectService.GetObject(addedObject);
				tasks.AddLast(grainReference.Subscribe(this));

				GetLogger().Debug($"{nameof(Unit)} {State.Id} subbed to {addedObject}");
			}

			// is this the most efficient way to set workingSet?
			workingSet.Clear();
			workingSet.UnionWith(newWorkingSet);

			await Task.WhenAll(tasks);
		}

		public virtual void HandleObjectCreate(ObjectEntity objectEntity, CreationUpdate update)
			=> GetLogger().Debug($"{nameof(Unit)} {State.Id} sees creation of object {objectEntity.Id}");

		public virtual void HandleObjectUpdate(ObjectEntity objectEntity, ObjectUpdate update)
			=> GetLogger().Debug($"{nameof(Unit)} {State.Id} sees update of object {objectEntity.Id}");

		public virtual void HandleObjectDestroyed(ObjectEntity objectEntity)
		  => GetLogger().Debug($"{nameof(Unit)} {State.Id} sees destruction of object {objectEntity.Id}");
	}
}
