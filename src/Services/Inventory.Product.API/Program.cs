using Common.Logging;
using Product.API.Extensions;
using Serilog;

var builder = WebApplication.CreateBuilder(args);

Log.Information("Start Inventory Product API up");

try
{
    builder.Host.UseSerilog(Serilogger.Configure);
    builder.AddAppConfigurations();

    // Add services to the container.
    builder.Services.AddInfrastructure();

    var app = builder.Build();

    app.UseInfrastructure();
}
catch (Exception ex)
{
    Log.Fatal(ex, "Unhandled exception");
}
finally
{
    Log.Information("Shut down Inventory Product API complete");
    Log.CloseAndFlush();
}