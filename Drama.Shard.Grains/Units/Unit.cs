using Drama.Shard.Grains.Objects;
using Drama.Shard.Interfaces.Objects;
using Drama.Shard.Interfaces.Units;
using Orleans;
using System;
using System.Threading.Tasks;

namespace Drama.Shard.Grains.Units
{
	public sealed class Unit : AbstractUnit<UnitEntity>, IUnit
	{

	}

	public abstract class AbstractUnit<TEntity> : AbstractPersistentObject<TEntity>, IUnit<TEntity>
		where TEntity : UnitEntity, new()
	{
		protected override ObjectUpdateFlags UpdateFlags => base.UpdateFlags | ObjectUpdateFlags.Living;

		protected override MovementUpdate BuildMovementUpdate()
			=> new MovementUpdate(State, UpdateFlags);
	}
}
