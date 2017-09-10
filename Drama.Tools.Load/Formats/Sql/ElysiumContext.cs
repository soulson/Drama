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

using Drama.Tools.Load.Formats.Sql.Entities;
using Microsoft.EntityFrameworkCore;

namespace Drama.Tools.Load.Formats.Sql
{
	public sealed class ElysiumContext : DbContext
	{
		private readonly string address;
		private readonly int port;
		private readonly string schema;
		private readonly string user;
		private readonly string password;

		public ElysiumContext(string address, int port, string schema, string user, string password)
		{
			this.address = address;
			this.port = port;
			this.schema = schema;
			this.user = user;
			this.password = password;
		}

		protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
		{
			optionsBuilder.UseMySql($"Server={address};port={port};database={schema};uid={user};pwd={password};");
		}

		protected override void OnModelCreating(ModelBuilder modelBuilder)
		{
			modelBuilder.Entity<PlayerCreateInfo>()
				.HasKey("Race", "Class");
			modelBuilder.Entity<CreatureTemplate>()
				.HasKey("Id");
			modelBuilder.Entity<Creature>()
				.HasKey("Id");
		}
	}
}
