﻿using Orleans;
using System;
using System.Threading.Tasks;

namespace Drama.Shard.Interfaces.Objects
{
	public interface IPersistentObject : IPersistentObject<ObjectEntity>, IGrainWithIntegerKey
	{

	}

	public interface IPersistentObject<TEntity> : IGrainWithIntegerKey
		where TEntity : ObjectEntity, new()
	{
		Task<bool> Exists();
		Task<TEntity> GetEntity();
	}
}