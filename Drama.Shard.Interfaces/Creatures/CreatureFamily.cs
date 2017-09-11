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

namespace Drama.Shard.Interfaces.Creatures
{
	/// <summary>
	/// Defines Creature families. See CreatureFamily.dbc
	/// </summary>
	public enum CreatureFamily : int
	{
		None = 0,
		Wolf = 1,
		Cat = 2,
		Spider = 3,
		Bear = 4,
		Boar = 5,
		Crocolisk = 6,
		CarrionBird = 7,
		Crab = 8,
		Gorilla = 9,
		Raptor = 11,
		Tallstrider = 12,
		Felhunter = 15,
		Voidwalker = 16,
		Succubus = 17,
		Doomguard = 19,
		Scorpid = 20,
		Turtle = 21,
		Imp = 23,
		Bat = 24,
		Hyena = 25,
		Owl = 26,
		WindSerpent = 27,
		RemoteControl = 28,
	}
}
