using Drama.Core.Interfaces.Networking;
using Orleans;
using System;

namespace Drama.Auth.Interfaces.Session
{
  public interface IAuthSessionObserver : IGrainObserver
  {
    void ReceivePacket(IOutPacket packet);
  }
}
