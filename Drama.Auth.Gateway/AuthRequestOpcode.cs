using System;

namespace Drama.Auth.Gateway
{
  public enum AuthRequestOpcode : byte
  {
    LogonChallenge = 0x00,
    LogonProof = 0x01,
    ReconnectChallenge = 0x02,
    ReconnectProof = 0x03,
    RealmList = 0x10,
  }
}
