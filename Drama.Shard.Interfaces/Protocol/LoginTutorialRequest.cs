using System.IO;

namespace Drama.Shard.Interfaces.Protocol
{
	public sealed class LoginTutorialRequest : AbstractOutPacket
	{
		public override ShardServerOpcode Opcode => ShardServerOpcode.LoginTutorialFlags;

		public int[] Data { get; } = new int[8];

		public LoginTutorialRequest()
		{
			for (int i = 0; i < Data.Length; ++i)
				Data[i] = -1;
		}

		protected override void Write(BinaryWriter writer)
		{
			foreach (int i in Data)
				writer.Write(i);
		}
	}
}
