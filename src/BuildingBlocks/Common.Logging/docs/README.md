# 📘 Hướng Dẫn Logging Với Serilog và CorrelationId

## Mục tiêu
- Cấu hình logging chuẩn cho microservice sử dụng `Serilog`
- Tự động gắn `CorrelationId`, `FileName`, `LineNumber` vào log
- Sử dụng tiện ích `CustomLogging` để ghi log nhất quán

## Thành phần chính
- [CorrelationIdMiddleware.md](CorrelationIdMiddleware.md)
- [HttpContextProvider.md](HttpContextProvider.md)
- [CustomLogging.md](CustomLogging.md)
- [LoggingExtensions.md](LoggingExtensions.md)
- [LoggingOptions.md](LoggingOptions.md)

## Cách sử dụng trong service
1. **Register**:
   ```csharp
    builder.Host.UseSharedSerilog("Basket service");
    builder.Services.AddHttpContextAccessor();
   ```

2. **Config pipe line** 
   ```csharp
    var app = builder.Build();

    var httpContextProvider = app.Services.GetRequiredService<IHttpContextAccessor>();
    HttpContextProvider.Accessor = httpContextProvider;
   ```

3. **Ghi log**:
    a. Sử dụng `Log` không có `context`:
   ```csharp
    public static class CustomerEndpoint
    {
        public static void MapCustomerEndpoints(this IEndpointRouteBuilder app)
        {
            var group = app.MapGroup("/api/customer").WithTags("Customer");

            // GET: Get all customers
            group.MapGet("/",
            async ([FromServices] ICustomerRepository customerRepository) =>
            {
                var logger = Log.ForContext("SourceContext", nameof(CustomerEndpoint));

                logger.Info("Fetching all customers from the database");

                var customers = await customerRepository.FindAll().ToListAsync();

                logger.Info($"Fetched {customers.Count} customers");

                return Results.Ok(customers);
            })
            .WithName("GetAllCustomers")
            .WithDescription("Get all customers");
        }
    }
   ```