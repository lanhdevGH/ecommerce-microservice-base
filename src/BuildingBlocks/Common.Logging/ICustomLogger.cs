using System.Runtime.CompilerServices;

namespace Common.Logging;

public interface ICustomLogger<T>
{
    void Info(string message, [CallerFilePath] string filePath = "", [CallerLineNumber] int lineNumber = 0);
    void Warn(string message, [CallerFilePath] string filePath = "", [CallerLineNumber] int lineNumber = 0);
    void Debug(string message, [CallerFilePath] string filePath = "", [CallerLineNumber] int lineNumber = 0);
    void Err(string message, [CallerFilePath] string filePath = "", [CallerLineNumber] int lineNumber = 0);
    void Err(Exception ex, string? message = null, [CallerFilePath] string filePath = "", [CallerLineNumber] int lineNumber = 0);
    void Fatal(string message, [CallerFilePath] string filePath = "", [CallerLineNumber] int lineNumber = 0);
    void Fatal(Exception ex, string? message = null, [CallerFilePath] string filePath = "", [CallerLineNumber] int lineNumber = 0);
}
