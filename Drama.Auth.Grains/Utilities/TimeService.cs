using Drama.Auth.Interfaces.Utilities;
using Orleans;
using Orleans.Concurrency;
using System;
using System.Threading.Tasks;

namespace Drama.Auth.Grains.Utilities
{
	[StatelessWorker]
	public class TimeService : Grain, ITimeService
	{
		public Task<DateTime> GetNow()
			=> Task.FromResult(DateTime.Now);
	}
}
