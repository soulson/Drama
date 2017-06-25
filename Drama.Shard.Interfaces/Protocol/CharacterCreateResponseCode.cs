using System;

namespace Drama.Shard.Interfaces.Protocol
{
	public enum CharacterCreateResponseCode : byte
	{
		InProgress = 0x2D,
		Success = 0x2E,
		Error = 0x2F,
		Failed = 0x30,
		NameTaken = 0x31,
		Disabled = 0x3A,
		FactionViolation = 0x33,
		ServerLimit = 0x34,
		AccountLimit = 0x35,
	}
}
