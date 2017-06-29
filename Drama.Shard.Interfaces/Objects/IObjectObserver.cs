using Orleans;
using System;

namespace Drama.Shard.Interfaces.Objects
{
	public interface IObjectObserver : IGrainObserver
	{
		/// <summary>
		/// This method is called when an IPersistentObject's client-visible state has changed.
		/// </summary>
		/// <param name="objectId">the ID of the object that has updated</param>
		/// <param name="update">an ObjectUpdate describing what has changed</param>
		void HandleObjectUpdate(ObjectID objectId, ObjectUpdate update);
	}
}
