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
