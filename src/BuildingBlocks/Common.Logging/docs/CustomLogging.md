# CustomLogging

## Mục đích
Tiện ích mở rộng cho `ILogger` giúp ghi log có định dạng chuẩn hóa, tự động kèm:

- Tên file (`FileName`)
- Số dòng (`LineNumber`)
- `CorrelationId` từ `HttpContext`

## Các phương thức
- `Info`, `Warn`, `Debug`, `Err`, `Fatal`
- Overload hỗ trợ ghi `Exception`

## Ví dụ sử dụng
```csharp
_logger.Info("Processing started");
_logger.Err(new Exception("error"), "Something went wrong");
```

## Tự động enrich log
```csharp
using (LogContext.PushProperty("FileName", ...))
using (LogContext.PushProperty("LineNumber", ...))
using (LogContext.PushProperty("CorrelationId", ...))
```