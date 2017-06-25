using System;

namespace Drama.Shard.Interfaces.Protocol
{
	public enum AuthResponseCode : byte
	{
		Success = 0x0C,
		Failed = 0x0D,
		Rejected = 0x0E,
		BadServerProof = 0x0F,
		Unavailable = 0x10,
		SystemError = 0x11,
		BillingError = 0x12,
		BillingExpired = 0x13,
		VersionMismatch = 0x14,
		UnknownAccount = 0x15,
		BadCredentials = 0x16,
		SessionExpired = 0x17,
		ServerShuttingDown = 0x18,
		AlreadyLoggingIn = 0x19,
		LoginServerNotFound = 0x1A,
		WaitQueue = 0x1B,
		Banned = 0x1C,
		AlreadyOnline = 0x1D,
		NoTimeRemaining = 0x1E,
		DatabaseBusy = 0x1F,
		Suspended = 0x20,
		ParentalControl = 0x21,
	}
}
