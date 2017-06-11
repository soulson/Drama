using Orleans.Runtime;
using System;
using System.Runtime.CompilerServices;

namespace Drama.Auth.Interfaces
{
	public static class LogSponsor
	{
		public static void Debug(this Logger log, string message, [CallerLineNumber] int lineNumber = -1, [CallerMemberName] string callerName = "<noname>")
			=> log.TrackTrace($"{DateTime.Now:o} {callerName}@{lineNumber}: {message}", Severity.Verbose);

		public static void Info(this Logger log, string message, [CallerLineNumber] int lineNumber = -1, [CallerMemberName] string callerName = "<noname>")
			=> log.TrackTrace($"{DateTime.Now:o} {callerName}@{lineNumber}: {message}", Severity.Info);

		public static void Warn(this Logger log, string message, [CallerLineNumber] int lineNumber = -1, [CallerMemberName] string callerName = "<noname>")
			=> log.TrackTrace($"{DateTime.Now:o} {callerName}@{lineNumber}: {message}", Severity.Warning);

		public static void Error(this Logger log, string message, [CallerLineNumber] int lineNumber = -1, [CallerMemberName] string callerName = "<noname>")
			=> log.TrackTrace($"{DateTime.Now:o} {callerName}@{lineNumber}: {message}", Severity.Error);
	}
}
