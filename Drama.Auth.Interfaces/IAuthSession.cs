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

    Task SubmitLogonChallenge(LogonChallengeRequest packet);
    Task SubmitLogonProof(LogonProofRequest packet);
    Task GetRealmList(RealmListRequest packet);
  }
}
