using System;

namespace Drama.Shard.Interfaces.Protocol
{
	[AttributeUsage(AttributeTargets.Class | AttributeTargets.Struct, Inherited = false, AllowMultiple = false)]
	public sealed class ClientPacketAttribute : Attribute
	{
		public ShardClientOpcode Opcode { get; }

		public ClientPacketAttribute(ShardClientOpcode opcode)
		{
			Opcode = opcode;
		}
	}
}
