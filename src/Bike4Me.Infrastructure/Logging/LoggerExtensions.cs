using Microsoft.Extensions.Logging;
using StackExchange.Redis;

namespace Bike4Me.Infrastructure.Logging;

public static class LoggerExtensions
{
    private static readonly Action<ILogger, string, string, string, int, int, Exception> _exceptionOnRetries;
    private static readonly Action<ILogger, string, RedisConnectionException> _redisException;
    private static readonly Action<ILogger, string, string, string?, Exception> _handlingMessageException;
    private static readonly Action<ILogger, string, string, string, string?, Exception> _handlingEventException;
    private static readonly Action<ILogger, string, string, Exception?> _informationMessage;
    private static readonly Action<ILogger, string, string, Exception?> _warningMessage;
    private static readonly Action<ILogger, string, string, Exception?> _errorMessage;
    private static readonly Action<ILogger, string, string, Exception> _criticalMessage;

    static LoggerExtensions()
    {
        _exceptionOnRetries = LoggerMessage.Define<string, string, string, int, int>(
            logLevel: LogLevel.Warning,
            eventId: new EventId(id: 1, name: "Bike4Me.API"),
            formatString: "[{Prefix}] Exception {ExceptionType} with message {Message} detected on attempt {Retry} of {Retries}");

        _redisException = LoggerMessage.Define<string>(
            logLevel: LogLevel.Error,
            eventId: new EventId(id: 2, name: "Bike4Me.API"),
            formatString: "Redis exception with message: {Message}");

        _handlingEventException = LoggerMessage.Define<string, string, string, string?>(
                logLevel: LogLevel.Error,
                eventId: 3,
                formatString: "[{context}] - ERROR handling event: {eventName} with message: '{message}' on {stackTrace}.");

        _handlingMessageException = LoggerMessage.Define<string, string, string?>(
                logLevel: LogLevel.Error,
                eventId: 4,
                formatString: "[{context}] - ERROR handling message: {message} - StackTrace: {stackTrace}.");

        _informationMessage = LoggerMessage.Define<string, string>(
               logLevel: LogLevel.Information,
               eventId: 5,
               formatString: "[{context}] - {message}.");

        _warningMessage = LoggerMessage.Define<string, string>(
            logLevel: LogLevel.Warning,
            eventId: 6,
            formatString: "[{context}] - {message}.");

        _errorMessage = LoggerMessage.Define<string, string>(
               logLevel: LogLevel.Error,
               eventId: 7,
               formatString: "[{context}] - {message}.");

        _criticalMessage = LoggerMessage.Define<string, string>(
               logLevel: LogLevel.Critical,
               eventId: 8,
               formatString: "[{context}] - {message}.");
    }

    public static void LogExceptionOnRetry(this ILogger logger, Exception exception, string prefix, int retry, int retries)
    {
        _exceptionOnRetries(logger, prefix, exception.GetType().Name, exception.Message, retry, retries, exception);
    }

    public static void LogRedisException(this ILogger logger, RedisConnectionException exception)
    {
        _redisException(logger, exception.Message, exception);
    }

    public static void LogHandlingMessageException<T>(this ILogger logger, Exception exception)
            where T : class
    {
        _handlingMessageException(logger, typeof(T).Name, exception.Message, exception.StackTrace, exception);
    }

    public static void LogHandlingEventException<T>(this ILogger logger, string eventName, Exception exception)
          where T : class
    {
        _handlingEventException(logger, typeof(T).Name, eventName, exception.Message, exception.StackTrace, exception);
    }

    public static void LogInformation<T>(this ILogger logger, string message) where T : class
    {
        _informationMessage(logger, typeof(T).Name, message, null);
    }

    public static void LogWarning<T>(this ILogger logger, string message) where T : class
    {
        _warningMessage(logger, typeof(T).Name, message, null);
    }

    public static void LogWarning<T>(this ILogger logger, string message, Exception exception) where T : class
    {
        _warningMessage(logger, typeof(T).Name, message, exception);
    }

    public static void LogError<T>(this ILogger logger, string message) where T : class
    {
        _errorMessage(logger, typeof(T).Name, message, null);
    }

    public static void LogError<T>(this ILogger logger, string message, Exception exception) where T : class
    {
        _errorMessage(logger, typeof(T).Name, message, exception);
    }

    public static void LogCritical<T>(this ILogger logger, string message) where T : class
    {
        _criticalMessage(logger, typeof(T).Name, message, null);
    }

    public static void LogCritical<T>(this ILogger logger, Exception exception) where T : class
    {
        _criticalMessage(logger, typeof(T).Name, exception.Message, exception);
    }
}