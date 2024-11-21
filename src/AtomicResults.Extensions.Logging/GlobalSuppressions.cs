// This file is used by Code Analysis to maintain SuppressMessage
// attributes that are applied to this project.
// Project-level suppressions either have no target or are given
// a specific target and scoped to a namespace, type, member, etc.

using System.Diagnostics.CodeAnalysis;

[assembly: SuppressMessage("Usage", "CA2254:Template should be a static expression",
    Justification = "The message template is dynamic and provided by the IError implementation.",
    Scope = "member",
    Target = "~M:AtomicResults.Extensions.Logging.LoggerExtensions.Log(Microsoft.Extensions.Logging.ILogger,Microsoft.Extensions.Logging.LogLevel,Microsoft.Extensions.Logging.EventId,System.Exception,AtomicResults.Abstractions.IError)")]
