using Drama.Shard.Grains.Objects;
using Drama.Shard.Interfaces.Units;
using System;

namespace Drama.Shard.Grains.Units
{
	public sealed class Unit : AbstractUnit<UnitEntity>, IUnit
	{

	}

	public abstract class AbstractUnit<TEntity> : AbstractPersistentObject<TEntity>, IUnit<TEntity>
		where TEntity : UnitEntity, new()
	{
	}
}
