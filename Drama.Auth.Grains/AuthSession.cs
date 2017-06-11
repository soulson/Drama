using Drama.Auth.Interfaces;
using Orleans;
using System;
using System.Threading.Tasks;
using Drama.Auth.Interfaces.Packets;

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
      GetLogger().Info($"{this.GetPrimaryKey()} connected");
      return Task.CompletedTask;
    }

    public Task Disconnect(IAuthSessionObserver observer)
    {
      sessionObservers.Unsubscribe(observer);
      GetLogger().Info($"{this.GetPrimaryKey()} disconnected");
      return Task.CompletedTask;
    }

    public Task GetRealmList(RealmListRequest packet)
    {
      GetLogger().Info("got realm list request");
      return Task.CompletedTask;
    }

    public Task SubmitLogonChallenge(LogonChallengeRequest packet)
    {
      GetLogger().Info($"got logon challenge request from {packet.Identity}");
      return Task.CompletedTask;
    }

    public Task SubmitLogonProof(LogonProofRequest packet)
    {
      GetLogger().Info("got logon proof request");
      return Task.CompletedTask;
    }
  }
}
