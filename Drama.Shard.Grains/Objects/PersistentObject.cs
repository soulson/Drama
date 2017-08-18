﻿/* 
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
using Drama.Shard.Interfaces.Objects;
using Orleans;
using System;
using System.Threading.Tasks;

namespace Drama.Shard.Grains.Objects
{
	public sealed class PersistentObject : AbstractPersistentObject<ObjectEntity>, IPersistentObject
	{

	}

	public abstract class AbstractPersistentObject<TEntity> : Grain<TEntity>, IPersistentObject<TEntity>
		where TEntity : ObjectEntity, new()
	{
		// TODO: make this a configuration item, or make it a virtual property
		/// <summary>
		/// PersistentObjects will notify subscribed PersistentObjects of updates
		/// to their state with the frequency established by this value.
		/// </summary>
		private const double UpdatePeriodSeconds = 1.0 / 20.0;

		/// <summary>
		/// True if this object has been created.
		/// Dead and despawned objects are still 'created', and so this is still true in those cases.
		/// </summary>
		protected virtual bool IsExists => State.Enabled;

		/// <summary>
		/// True if this object's client-visible state has been modified since the last UpdateObservers tick.
		/// Inheriting classes may either set this value directly or override its behavior based on other properties.
		/// UpdateObservers will set the value to false after notifying observers of all updates.
		/// </summary>
		protected virtual bool IsValuesUpdated { get; set; } = false;

		/// <summary>
		/// True if this object's position or (unit's) movement state has been modified since the last UpdateObservers tick.
		/// Inheriting classes may either set this value directly or override its behavior based on other properties.
		/// UpdateObservers will set the value to false after notifying observers of all updates.
		/// </summary>
		protected virtual bool IsMovementUpdated { get; set; } = false;

		/// <summary>
		/// Specifies which ObjectUpdate blocks will be included when notifying observers of updates.
		/// </summary>
		protected virtual ObjectUpdateFlags UpdateFlags => ObjectUpdateFlags.All | ObjectUpdateFlags.HasPosition;

		private IDisposable updateTimerHandle;
		private readonly ObserverSubscriptionManager<IObjectObserver> observerManager;

		public AbstractPersistentObject()
		{
			observerManager = new ObserverSubscriptionManager<IObjectObserver>();
		}

		public override Task OnActivateAsync()
		{
			updateTimerHandle = RegisterTimer(UpdateObservers, null, TimeSpan.FromSeconds(UpdatePeriodSeconds), TimeSpan.FromSeconds(UpdatePeriodSeconds));
			return base.OnActivateAsync();
		}

		public override Task OnDeactivateAsync()
		{
			if (observerManager.Count > 0)
				GetLogger().Warn($"observer manager for {GetType().Name} {this.GetPrimaryKeyLong()} still has {observerManager.Count} subscriptions at deactivation");

			observerManager.Clear();
			updateTimerHandle?.Dispose();
			return base.OnDeactivateAsync();
		}

		/// <remarks>
		/// This method gets invoked by the update timer every UpdatePeriodSeconds.
		/// Use it to send object updates to subscribers. The argument is always null.
		/// </remarks>
		private Task UpdateObservers(object arg)
		{
			if (IsValuesUpdated || IsMovementUpdated)
			{
				MovementUpdate movementUpdate = null;
				ValuesUpdate valuesUpdate = null;

				if (IsValuesUpdated)
				{
					valuesUpdate = BuildValuesUpdate(State, false);
					IsValuesUpdated = false;
				}
				if (IsMovementUpdated)
				{
					movementUpdate = BuildMovementUpdate(State);
					IsMovementUpdated = false;
				}

				var objectUpdate = new ObjectUpdate(State.Id, State.TypeId, UpdateFlags, movementUpdate, valuesUpdate);
				observerManager.Notify(observer => observer.HandleObjectUpdate(State, objectUpdate));
			}
			return Task.CompletedTask;
		}

		public Task<bool> Exists()
			=> Task.FromResult(IsExists);

		public Task<TEntity> GetEntity()
		{
			VerifyExists();

			return Task.FromResult(State);
		}

		public Task Subscribe(IObjectObserver observer)
		{
			VerifyExists();

			if (observerManager.IsSubscribed(observer))
				GetLogger().Warn($"observer {observer} tried to subscribe to {GetType().Name} {this.GetPrimaryKeyLong()} more than once");
			else
				observerManager.Subscribe(observer);

			return Task.CompletedTask;
		}

		public Task Unsubscribe(IObjectObserver observer)
		{
			VerifyExists();

			if (!observerManager.IsSubscribed(observer))
				GetLogger().Warn($"observer {observer} tried to unsubscribe from {GetType().Name} {this.GetPrimaryKeyLong()} but was not subscribed");
			else
				observerManager.Unsubscribe(observer);

			return Task.CompletedTask;
		}

		public Task Destroy()
		{
			VerifyExists();

			observerManager.Notify(observer => observer.HandleObjectDestroyed(State));
			return Task.CompletedTask;
		}

		public virtual Task<bool> IsIngame()
			=> Task.FromResult(true);

		protected virtual ValuesUpdate BuildValuesUpdate(ObjectEntity entity, bool creating)
			=> new ValuesUpdate(entity, creating);

		protected virtual MovementUpdate BuildMovementUpdate(ObjectEntity entity)
			=> new MovementUpdate(entity);

		protected CreationUpdate GetCreationUpdate(ObjectEntity entity)
			=> new CreationUpdate(State.Id, State.TypeId, UpdateFlags, BuildMovementUpdate(entity), BuildValuesUpdate(entity, true));

		protected void VerifyExists()
		{
			if (!IsExists)
				throw new ObjectDoesNotExistException($"{GetType().Name} {this.GetPrimaryKeyLong()} does not exist");
		}
	}
}
