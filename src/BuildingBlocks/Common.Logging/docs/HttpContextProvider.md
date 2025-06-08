# HttpContextProvider

## Mục đích
Cho phép truy cập `HttpContext` toàn cục để lấy `CorrelationId` trong `CustomLogging`.

## Cấu trúc
```csharp
public static IHttpContextAccessor? Accessor { get; set; }
```

## Khởi tạo trong Startup
```csharp
services.AddHttpContextAccessor();
HttpContextProvider.Accessor = services.BuildServiceProvider().GetService<IHttpContextAccessor>();
```