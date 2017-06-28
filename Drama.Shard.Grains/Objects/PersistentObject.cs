using Drama.Shard.Interfaces.Objects;
using Orleans;
using System.Threading.Tasks;

namespace Drama.Shard.Grains.Objects
{
	public sealed class PersistentObject : AbstractPersistentObject<ObjectEntity>, IPersistentObject
	{

	}

	public abstract class AbstractPersistentObject<TEntity> : Grain<TEntity>, IPersistentObject<TEntity>
		where TEntity : ObjectEntity, new()
	{
		protected virtual bool IsExists => State.Enabled;

		public Task<bool> Exists()
			=> Task.FromResult(IsExists);

		public Task<TEntity> GetEntity()
		{
			if (!IsExists)
				throw new ObjectDoesNotExistException($"{GetType().Name} {this.GetPrimaryKeyString()} does not exist");

			return Task.FromResult(State);
		}
	}
}
