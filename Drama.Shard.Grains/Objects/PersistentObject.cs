using Drama.Shard.Interfaces.Objects;
using Orleans;
using System.Threading.Tasks;
using System;
using Drama.Auth.Interfaces;

namespace Drama.Shard.Grains.Objects
{
	public sealed class PersistentObject : AbstractPersistentObject<ObjectEntity>, IPersistentObject
	{

	}

	public abstract class AbstractPersistentObject<TEntity> : Grain<TEntity>, IPersistentObject<TEntity>
		where TEntity : ObjectEntity, new()
	{
		// TODO: make this a configuration item
		private const double UpdatePeriodSeconds = 1.0 / 60.0;

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
		protected virtual bool IsUpdated { get; set; } = false;

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
			RegisterTimer(UpdateObservers, null, TimeSpan.FromSeconds(UpdatePeriodSeconds), TimeSpan.FromSeconds(UpdatePeriodSeconds));
			return base.OnActivateAsync();
		}

		public override Task OnDeactivateAsync()
		{
			updateTimerHandle?.Dispose();
			return base.OnDeactivateAsync();
		}

		/// <remarks>
		/// This method gets invoked by the update timer every UpdatePeriodSeconds.
		/// Use it to send object updates to subscribers. The argument is always null.
		/// </remarks>
		private Task UpdateObservers(object arg)
		{
			if (IsUpdated)
			{
				IsUpdated = false;
			}
			return Task.CompletedTask;
		}

		public Task<bool> Exists()
			=> Task.FromResult(IsExists);

		public Task<TEntity> GetEntity()
		{
			if (!IsExists)
				throw new ObjectDoesNotExistException($"{GetType().Name} {this.GetPrimaryKeyLong()} does not exist");

			return Task.FromResult(State);
		}

		public Task Subscribe(IObjectObserver observer)
		{
			if (observerManager.IsSubscribed(observer))
				GetLogger().Warn($"observer {observer} tried to subscribe to {GetType().Name} {this.GetPrimaryKeyLong()} more than once");
			else
				observerManager.Subscribe(observer);

			return Task.CompletedTask;
		}

		public Task Unsubscribe(IObjectObserver observer)
		{
			if (!observerManager.IsSubscribed(observer))
				GetLogger().Warn($"observer {observer} tried to unsubscribe from {GetType().Name} {this.GetPrimaryKeyLong()} but was not subscribed");
			else
				observerManager.Unsubscribe(observer);

			return Task.CompletedTask;
		}
	}
}
