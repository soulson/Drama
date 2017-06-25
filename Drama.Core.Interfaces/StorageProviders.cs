using System;

namespace Drama.Core.Interfaces
{
	public static class StorageProviders
	{
		/// <summary>
		/// This storage provider stores things like which shards exist and how to address them.
		/// </summary>
		public const string Infrastructure = "DeploymentStore";

		/// <summary>
		/// This storage provider stores account information.
		/// </summary>
		public const string Account = "AccountStore";

		/// <summary>
		/// This storage provider stores frequently-changing data such as characters and item
		/// instances.
		/// </summary>
		public const string DynamicWorld = "DynamicWorldStore";

		/// <summary>
		/// This storage provider stores infrequently-changing data such as mob spawn points
		/// and item definitions.
		/// </summary>
		public const string StaticWorld = "StaticWorldStore";
	}
}
