using Drama.Auth.Interfaces;
using Orleans;
using System;
using System.Threading.Tasks;

namespace Drama.Auth.Grains
{
  public class AuthSession : Grain, IAuthSession
  {
    private readonly ObserverSubscriptionManager<IAuthSessionObserver> sessionObservers;

    public AuthSession()
    {
      sessionObservers = new ObserverSubscriptionManager<IAuthSessionObserver>();
    }

    public Task Connect(IAuthSessionObserver observer)
    {
      sessionObservers.Subscribe(observer);
      GetLogger().TrackTrace($"{this.GetPrimaryKey()} connected", Orleans.Runtime.Severity.Info);
      Console.WriteLine($"{this.GetPrimaryKey()} connected");
      return Task.CompletedTask;
    }

    public Task Disconnect(IAuthSessionObserver observer)
    {
      sessionObservers.Unsubscribe(observer);
      GetLogger().TrackTrace($"{this.GetPrimaryKey()} disconnected", Orleans.Runtime.Severity.Info);
      Console.WriteLine($"{this.GetPrimaryKey()} disconnected");
      return Task.CompletedTask;
    }
  }
}
