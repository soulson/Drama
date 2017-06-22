﻿using Drama.Auth.Interfaces.Protocol;
using Orleans;
using System;
using System.Threading.Tasks;

namespace Drama.Auth.Interfaces.Session
{
  public interface IAuthSession : IGrainWithGuidKey
  {
    Task Connect();
    Task Disconnect();

    Task<LogonChallengeResponse> SubmitLogonChallenge(LogonChallengeRequest packet);
    Task<LogonProofResponse> SubmitLogonProof(LogonProofRequest packet);
    Task<RealmListResponse> GetRealmList(RealmListRequest packet);
  }
}
