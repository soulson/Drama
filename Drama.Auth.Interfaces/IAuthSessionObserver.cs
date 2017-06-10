using Drama.Core.Interfaces.Networking;
using Orleans;
using System;

namespace Drama.Auth.Interfaces
{
  public interface IAuthSessionObserver : IGrainObserver
  {
    void ReceivePacket(IPacket packet);
  }
}
