using System;

namespace Drama.Auth.Interfaces.Shard
{
	public class ShardEntity
	{
		public bool Enabled { get; set; }
		public string Name { get; set; }
		public string Address { get; set; }
		public int Port { get; set; }
		public ShardType ShardType { get; set; }
		public ShardFlags ShardFlags { get; set; }
	}
}
