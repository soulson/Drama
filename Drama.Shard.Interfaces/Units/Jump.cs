using System;

namespace Drama.Shard.Interfaces.Units
{
	public class Jump
	{
		public float Velocity { get; set; }
		public float SineAngle { get; set; }
		public float CosineAngle { get; set; }
		public float XYSpeed { get; set; }

		public void Clear()
		{
			Velocity = 0.0f;
			SineAngle = 0.0f;
			CosineAngle = 0.0f;
			XYSpeed = 0.0f;
		}
	}
}
