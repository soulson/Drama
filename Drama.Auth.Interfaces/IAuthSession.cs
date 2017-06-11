using Drama.Auth.Interfaces.Packets;
using Orleans;
using System;
using System.Threading.Tasks;

namespace Drama.Auth.Interfaces
{
  public interface IAuthSession : IGrainWithGuidKey
  {
    Task Connect(IAuthSessionObserver observer);
    Task Disconnect(IAuthSessionObserver observer);

    Task SubmitLogonChallenge(LogonChallenge packet);
    Task SubmitLogonProof(LogonProof packet);
    Task GetRealmList(RealmList packet);
  }
}
