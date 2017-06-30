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
		// TODO: make this a configuration item
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
					valuesUpdate = BuildValuesUpdate(false);
					IsValuesUpdated = false;
				}
				if (IsMovementUpdated)
				{
					movementUpdate = BuildMovementUpdate();
					IsMovementUpdated = false;
				}

				var objectUpdate = new ObjectUpdate(State.Id, State.TypeId, UpdateFlags, movementUpdate, valuesUpdate);
				observerManager.Notify(observer => observer.HandleObjectUpdate(State.Id, objectUpdate));
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

		public Task<CreationUpdate> GetCreationUpdate()
		{
			VerifyExists();

			return Task.FromResult(new CreationUpdate(State.Id, State.TypeId, UpdateFlags, BuildMovementUpdate(), BuildValuesUpdate(true)));
		}

		protected virtual ValuesUpdate BuildValuesUpdate(bool creating)
			=> new ValuesUpdate(State, creating);

		protected virtual MovementUpdate BuildMovementUpdate()
			=> new MovementUpdate(State);

		protected void VerifyExists()
		{
			if (!IsExists)
				throw new ObjectDoesNotExistException($"{GetType().Name} {this.GetPrimaryKeyLong()} does not exist");
		}
	}
}
