/* 
 * The Drama project: what you get when a bunch of actors try to host a game.
 * Copyright (C) 2017 Soulson
 *
 * This program is free software: you can redistribute it and/or modify
 * it under the terms of the GNU Affero General Public License as
 * published by the Free Software Foundation, either version 3 of the
 * License, or (at your option) any later version.
 *
 * This program is distributed in the hope that it will be useful,
 * but WITHOUT ANY WARRANTY; without even the implied warranty of
 * MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
 * GNU Affero General Public License for more details.
 *
 * You should have received a copy of the GNU Affero General Public License
 * along with this program.  If not, see <http://www.gnu.org/licenses/>.
 */

using System;

namespace Drama.Core.Interfaces
{
	public static class StorageProviders
	{
		/// <summary>
		/// This storage provider stores things like which shards exist and how to address them.
		/// </summary>
		public const string Infrastructure = "InfrastructureStore";

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
