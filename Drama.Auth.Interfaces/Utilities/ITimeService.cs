using Orleans;
using System;
using System.Threading.Tasks;

namespace Drama.Auth.Interfaces.Utilities
{
	public interface ITimeService : IGrainWithIntegerKey
	{
		Task<DateTime> GetNow();
	}
}
