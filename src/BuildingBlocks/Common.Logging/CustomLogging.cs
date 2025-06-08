using Serilog;
using Serilog.Context;
using System.Runtime.CompilerServices;

namespace Common.Logging;

public static class CustomLogging
{
    public static void Info(this ILogger logger, string message,
        [CallerFilePath] string filePath = "", 
        [CallerLineNumber] int lineNumber = 0) 
        => WithLogContext(Path.GetFileName(filePath), lineNumber, logger.Information, message);

    public static void Warn(this ILogger logger, string message,
        [CallerFilePath] string filePath = "", 
        [CallerLineNumber] int lineNumber = 0) 
        => WithLogContext(Path.GetFileName(filePath), lineNumber, logger.Warning, message);

    public static void Err(this ILogger logger, string message,
        [CallerFilePath] string filePath = "", 
        [CallerLineNumber] int lineNumber = 0) 
        => WithLogContext(Path.GetFileName(filePath), lineNumber, logger.Error, message);

    public static void Debug(this ILogger logger, string message,
        [CallerFilePath] string filePath = "", 
        [CallerLineNumber] int lineNumber = 0) 
        => WithLogContext(Path.GetFileName(filePath), lineNumber, logger.Debug, message);

    public static void Fatal(this ILogger logger, string message,
        [CallerFilePath] string filePath = "", 
        [CallerLineNumber] int lineNumber = 0) 
        => WithLogContext(Path.GetFileName(filePath), lineNumber, logger.Fatal, message);

    public static void Err(this ILogger logger, 
        Exception ex, 
        string? message = null,
        [CallerFilePath] string filePath = "", 
        [CallerLineNumber] int lineNumber = 0)
    {
        var fileName = Path.GetFileName(filePath);
        var correlationId = TryGetCorrelationId() ?? "N/A";
        using (LogContext.PushProperty("FileName", fileName))
        using (LogContext.PushProperty("LineNumber", lineNumber))
        {
            logger.Error(ex, $"[{fileName}:{lineNumber}] - CorrelationId: {correlationId} - {message ?? ex.Message}");
        }
    }

    public static void Fatal(this ILogger logger, 
        Exception ex, 
        string? message = null,
        [CallerFilePath] string filePath = "", 
        [CallerLineNumber] int lineNumber = 0)
    {
        var fileName = Path.GetFileName(filePath);
        var correlationId = TryGetCorrelationId() ?? "N/A";
        using (LogContext.PushProperty("FileName", fileName))
        using (LogContext.PushProperty("LineNumber", lineNumber))
        {
            logger.Fatal(ex, $"[{fileName}:{lineNumber}] - CorrelationId: {correlationId} - {message ?? ex.Message}");
        }
    }

    private static string? TryGetCorrelationId()
    {
        var accessor = HttpContextProvider.Accessor;
        return accessor?.HttpContext?.Items["CorrelationId"]?.ToString();
    }

    private static void WithLogContext(string fileName, int lineNumber, Action<string> logAction, string message)
    {
        var correlationId = TryGetCorrelationId() ?? "N/A";
        using (LogContext.PushProperty("FileName", fileName))
        using (LogContext.PushProperty("LineNumber", lineNumber))
        {
            logAction($"[{fileName}:{lineNumber}] - CorrelationId: {correlationId} - {message}");
        }
    }
}
