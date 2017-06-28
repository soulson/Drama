using Drama.Shard.Interfaces.Objects;
using Orleans;
using System;

namespace Drama.Shard.Interfaces.Units
{
	public interface IUnit : IUnit<UnitEntity>, IGrainWithIntegerKey
	{

	}

	public interface IUnit<TEntity> : IPersistentObject<TEntity>, IGrainWithIntegerKey
		where TEntity : UnitEntity, new()
	{

	}
}
