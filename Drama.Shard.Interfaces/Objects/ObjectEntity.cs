using Drama.Core.Interfaces.Numerics;
using System;

namespace Drama.Shard.Interfaces.Objects
{
	public class ObjectEntity
	{
		public bool Enabled { get; set; }

		public string Shard { get; set; }
		public ObjectID Id { get; set; }

		public Vector3 Position { get; set; }
		public float Orientation { get; set; }
		public int MapId { get; set; }
	}
}
