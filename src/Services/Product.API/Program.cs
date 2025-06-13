using Common.Logging.Serilog;
using Product.API;
using Product.API.Extensions;
using Product.API.Persistences;
using Serilog;

var builder = WebApplication.CreateBuilder(args);
builder.Host.UseSharedSerilog("Product service");

Log.Information("Start Product API up");

try
{
    builder.AddAppConfigurations();
    // Add services to the container.
    builder.Services.AddInfrastructure(builder.Configuration);
    builder.Services.AddAutoMapper(typeof(MappingProfile));

    var app = builder.Build();
    app.UseInfrastructure();    

    app.MigrateDatabase<ProductContext>((context, _) =>
    {
        // Use the Microsoft.Extensions.Logging.ILogger instead of Serilog.ILogger
        var loggerFactory = app.Services.GetRequiredService<ILoggerFactory>();
        var logger = loggerFactory.CreateLogger<ProductContextSeed>();
        ProductContextSeed.SeedProductAsync(context, logger).Wait();
    })
    .Run();
}
catch (Exception ex)
{
    string type = ex.GetType().Name;
    if (type.Equals("StopTheHostException", StringComparison.Ordinal))
    {
        throw;
    }

    Log.Fatal(ex, $"Unhandled exception: {ex.Message}");
}
finally
{
    Log.Information("Shut down Product API complete");
    Log.CloseAndFlush();
}