using System;

namespace Drama.Shard.Interfaces.Units
{
	public class MovementSpeed
	{
		public MovementSpeed()
		{
			Walking = 2.5f;
			Running = 7.0f;
			RunningBack = 4.5f;
			Swimming = 4.722222f;
			SwimmingBack = 2.5f;
			Turning = 3.141594f;
		}

		public float Walking { get; set; }
		public float Running { get; set; }
		public float RunningBack { get; set; }
		public float Swimming { get; set; }
		public float SwimmingBack { get; set; }
		public float Turning { get; set; }
	}
}
