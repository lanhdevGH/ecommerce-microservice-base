using Serilog;
using Serilog.Context;
using System.Runtime.CompilerServices;

namespace Common.Logging.Serilog;

public class SerilogCustomLogger<T> : ICustomLogger<T>
{
    private readonly ILogger _logger;

    public SerilogCustomLogger()
    {
        _logger = Log.ForContext<T>();
    }

    public void Info(string message, [CallerFilePath] string filePath = "", [CallerLineNumber] int lineNumber = 0)
        => LogWithContext(_logger.Information, message, filePath, lineNumber);

    public void Warn(string message, [CallerFilePath] string filePath = "", [CallerLineNumber] int lineNumber = 0)
        => LogWithContext(_logger.Warning, message, filePath, lineNumber);

    public void Debug(string message, [CallerFilePath] string filePath = "", [CallerLineNumber] int lineNumber = 0)
        => LogWithContext(_logger.Debug, message, filePath, lineNumber);

    public void Err(string message, [CallerFilePath] string filePath = "", [CallerLineNumber] int lineNumber = 0)
        => LogWithContext(_logger.Error, message, filePath, lineNumber);

    public void Fatal(string message, [CallerFilePath] string filePath = "", [CallerLineNumber] int lineNumber = 0)
        => LogWithContext(_logger.Fatal, message, filePath, lineNumber);

    public void Err(Exception ex, string? message = null, [CallerFilePath] string filePath = "", [CallerLineNumber] int lineNumber = 0)
        => LogExceptionWithContext(_logger.Error, ex, message, filePath, lineNumber);

    public void Fatal(Exception ex, string? message = null, [CallerFilePath] string filePath = "", [CallerLineNumber] int lineNumber = 0)
        => LogExceptionWithContext(_logger.Fatal, ex, message, filePath, lineNumber);



    private void LogWithContext(Action<string> logAction, string message,
        string filePath, int lineNumber)
    {
        var fileName = Path.GetFileName(filePath);
        var correlationId = TryGetCorrelationId() ?? "N/A";

        using (LogContext.PushProperty("FileName", fileName))
        using (LogContext.PushProperty("LineNumber", lineNumber))
        using (LogContext.PushProperty("CorrelationId", correlationId))
        {
            logAction($"[{fileName}:{lineNumber}] - CorrelationId: {correlationId} - {message}");
        }
    }

    private void LogExceptionWithContext(Action<Exception, string> logAction, Exception ex, string? message,
        string filePath, int lineNumber)
    {
        var fileName = Path.GetFileName(filePath);
        var correlationId = TryGetCorrelationId() ?? "N/A";

        using (LogContext.PushProperty("FileName", fileName))
        using (LogContext.PushProperty("LineNumber", lineNumber))
        using (LogContext.PushProperty("CorrelationId", correlationId))
        {
            logAction(ex, $"[{fileName}:{lineNumber}] - CorrelationId: {correlationId} - {message ?? ex.Message}");
        }
    }

    private static string? TryGetCorrelationId()
    {
        var accessor = HttpContextProvider.Accessor;
        return accessor?.HttpContext?.Items["CorrelationId"]?.ToString();
    }
}
