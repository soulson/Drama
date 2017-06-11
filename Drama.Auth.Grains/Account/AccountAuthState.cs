using System;

namespace Drama.Auth.Grains.Account
{
	public enum AccountAuthState
	{
		Passivated = 0,
		ParametersGenerated = 1,
		Authenticated = 2,
	}
}
