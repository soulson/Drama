using System;

namespace Drama.Auth.Interfaces.Protocol
{
	public enum AuthResponse : byte
	{
		Success = 0x00,
		FailBanned = 0x03,
		FailBadCredentials = 0x04,
		FailBusy = 0x08,
		FailBadClientVersion = 0x09,
		FailSuspended = 0x0C,
	}
}
