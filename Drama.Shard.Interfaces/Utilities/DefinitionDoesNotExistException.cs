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

using Drama.Core.Interfaces;
using System;

namespace Drama.Shard.Interfaces.Utilities
{
	/// <summary>
	/// This exception is thrown when an attempt is made to access the entity of
	/// a definition/template that has not yet been created.
	/// </summary>
	public class DefinitionDoesNotExistException : DramaException
	{
		public DefinitionDoesNotExistException(string message) : base(message) { }
		public DefinitionDoesNotExistException(string message, Exception cause) : base(message, cause) { }
	}
}
