# CorrelationIdMiddleware

## Mục đích
Gắn `CorrelationId` vào mỗi HTTP request/response và enrich log.

## Chức năng
- Lấy `X-Correlation-ID` từ request header nếu có, nếu không thì tạo mới `Guid`.
- Gắn vào:
  - `context.Items["CorrelationId"]`
  - `context.Response.Headers["X-Correlation-ID"]`
  - `LogContext` của Serilog

## Cách sử dụng
```csharp
app.UseMiddleware<CorrelationIdMiddleware>();
```