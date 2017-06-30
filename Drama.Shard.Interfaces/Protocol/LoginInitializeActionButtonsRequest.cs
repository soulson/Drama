using System.IO;

namespace Drama.Shard.Interfaces.Protocol
{
	public sealed class LoginInitializeActionButtonsRequest : AbstractOutPacket
	{
		public override ShardServerOpcode Opcode => ShardServerOpcode.LoginInitializeActionButtons;

		public int[] ActionButtons { get; } = new int[120];

		protected override void Write(BinaryWriter writer)
		{
			// TODO: LoginInitializeActionButtons
			foreach (int i in ActionButtons)
				writer.Write(i);
		}
	}
}
