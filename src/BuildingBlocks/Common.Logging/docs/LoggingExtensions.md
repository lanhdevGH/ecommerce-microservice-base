# LoggingExtensions

## Mục đích
- Cấu hình Serilog
- Đăng ký middleware CorrelationId

## Phương thức chính
### UseSharedSerilog
```csharp
builder.Host.UseSharedSerilog("MyService");
```

### UseCorrelationIdLogging
```csharp
app.UseCorrelationIdLogging();
```

## Hiện tại chỉ ghi log ra Console
Có thể mở rộng để ghi log ra Elasticsearch hoặc Seq.