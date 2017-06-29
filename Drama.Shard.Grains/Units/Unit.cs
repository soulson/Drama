using Drama.Shard.Grains.Objects;
using Drama.Shard.Interfaces.Units;
using System;
using Drama.Shard.Interfaces.Objects;

namespace Drama.Shard.Grains.Units
{
	public sealed class Unit : AbstractUnit<UnitEntity>, IUnit
	{

	}

	public abstract class AbstractUnit<TEntity> : AbstractPersistentObject<TEntity>, IUnit<TEntity>
		where TEntity : UnitEntity, new()
	{
		protected override ObjectUpdateFlags UpdateFlags => base.UpdateFlags | ObjectUpdateFlags.Living;
	}
}
