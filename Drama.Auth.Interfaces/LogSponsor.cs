using Orleans.Runtime;
using System;

namespace Drama.Auth.Interfaces
{
  public static class LogSponsor
  {
    public static void Debug(this Logger log, string message)
    {
      log.TrackTrace(message, Severity.Verbose);
    }

    public static void Info(this Logger log, string message)
    {
      log.TrackTrace(message, Severity.Info);
    }

    public static void Warn(this Logger log, string message)
    {
      log.TrackTrace(message, Severity.Warning);
    }

    public static void Error(this Logger log, string message)
    {
      log.TrackTrace(message, Severity.Error);
    }
  }
}
