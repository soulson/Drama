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
using System.Collections.Concurrent;

namespace Drama.Core.Gateway.Utilities
{
	public class Pool<T>
  {
    protected Func<T> Generator { get; }
    protected ConcurrentBag<T> Bag { get; }

    public Pool(Func<T> generator)
    {
      Generator = generator ?? throw new ArgumentNullException(nameof(generator));
      Bag = new ConcurrentBag<T>();
    }

    public T CheckOut()
    {
      if (Bag.TryTake(out var @object))
        return @object;
      else
        return Generator();
    }

    public void CheckIn(T @object)
    {
      Bag.Add(@object);
    }
  }
}
