using AtomicResults.Abstractions;
using Microsoft.Extensions.Logging;

namespace AtomicResults.Extensions.Logging;
public static class LoggerExtensions
{
    #region Log

    public static void Log(this ILogger logger, LogLevel logLevel, EventId eventId, Exception? exception, IError? error)
    {
        // TODO: Use spread operator when issue is fixed; tracking link: https://github.com/dotnet/roslyn/issues/71788
        var metadata = error?.Metadata != null ? error.Metadata as object?[] ?? error.Metadata.ToArray() : [];
        logger.Log(logLevel, eventId, exception, error?.Message, metadata);
    }

    public static void Log(this ILogger logger, LogLevel logLevel, Exception? exception, IError? error)
        => logger.Log(logLevel, eventId: default, exception, error);

    public static void Log(this ILogger logger, LogLevel logLevel, EventId eventId, IError? error)
        => logger.Log(logLevel, eventId, exception: null, error);

    public static void Log(this ILogger logger, LogLevel logLevel, IError? error)
        => logger.Log(logLevel, exception: null, error);


    #endregion

    #region LogTrace

    public static void LogTrace(this ILogger logger, IError? error)
        => logger.Log(LogLevel.Trace, error);

    public static void LogTrace(this ILogger logger, Exception? exception, IError? error)
        => logger.Log(LogLevel.Trace, exception, error);

    public static void LogTrace(this ILogger logger, EventId eventId, IError? error)
        => logger.Log(LogLevel.Trace, eventId, error);

    public static void LogTrace(this ILogger logger, EventId eventId, Exception? exception, IError? error)
        => logger.Log(LogLevel.Trace, eventId, exception, error);

    #endregion

    #region LogDebug

    public static void LogDebug(this ILogger logger, IError? error)
        => logger.Log(LogLevel.Debug, error);

    public static void LogDebug(this ILogger logger, Exception? exception, IError? error)
        => logger.Log(LogLevel.Debug, exception, error);

    public static void LogDebug(this ILogger logger, EventId eventId, IError? error)
        => logger.Log(LogLevel.Debug, eventId, error);

    public static void LogDebug(this ILogger logger, EventId eventId, Exception? exception, IError? error)
        => logger.Log(LogLevel.Debug, eventId, exception, error);

    #endregion

    #region LogInformation

    public static void LogInformation(this ILogger logger, IError? error)
        => logger.Log(LogLevel.Information, error);

    public static void LogInformation(this ILogger logger, Exception? exception, IError? error)
        => logger.Log(LogLevel.Information, exception, error);

    public static void LogInformation(this ILogger logger, EventId eventId, IError? error)
        => logger.Log(LogLevel.Information, eventId, error);

    public static void LogInformation(this ILogger logger, EventId eventId, Exception? exception, IError? error)
        => logger.Log(LogLevel.Information, eventId, exception, error);

    #endregion

    #region LogWarning

    public static void LogWarning(this ILogger logger, IError? error)
        => logger.Log(LogLevel.Warning, error);

    public static void LogWarning(this ILogger logger, Exception? exception, IError? error)
        => logger.Log(LogLevel.Warning, exception, error);

    public static void LogWarning(this ILogger logger, EventId eventId, IError? error)
        => logger.Log(LogLevel.Warning, eventId, error);

    public static void LogWarning(this ILogger logger, EventId eventId, Exception? exception, IError? error)
        => logger.Log(LogLevel.Warning, eventId, exception, error);

    #endregion

    #region LogError

    public static void LogError(this ILogger logger, IError? error)
       => logger.Log(LogLevel.Error, error);

    public static void LogError(this ILogger logger, Exception? exception, IError? error)
        => logger.Log(LogLevel.Error, exception, error);

    public static void LogError(this ILogger logger, EventId eventId, IError? error)
        => logger.Log(LogLevel.Error, eventId, error);

    public static void LogError(this ILogger logger, EventId eventId, Exception? exception, IError? error)
        => logger.Log(LogLevel.Error, eventId, exception, error);

    #endregion

    #region LogCritical

    public static void LogCritical(this ILogger logger, IError? error)
       => logger.Log(LogLevel.Critical, error);

    public static void LogCritical(this ILogger logger, Exception? exception, IError? error)
        => logger.Log(LogLevel.Critical, exception, error);

    public static void LogCritical(this ILogger logger, EventId eventId, IError? error)
        => logger.Log(LogLevel.Critical, eventId, error);

    public static void LogCritical(this ILogger logger, EventId eventId, Exception? exception, IError? error)
        => logger.Log(LogLevel.Critical, eventId, exception, error);

    #endregion
}
