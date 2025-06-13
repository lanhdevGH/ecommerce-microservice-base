using Basket.API.Extensions;
using Common.Logging;
using Common.Logging.Serilog;
using Serilog;

var builder = WebApplication.CreateBuilder(args);
builder.Host.UseSharedSerilog("Basket service");

Log.Information("Start Basket API up");

try
{
    builder.Host.AddAppConfigurations();
    // Configure Serilog for the application
    builder.Host.UseSharedSerilog("Basket service");
    builder.Services.AddHttpContextAccessor();

    // Add services to the container.
    builder.Services.ConfigureRedis(builder.Configuration);
    builder.Services.ConfigureServices();

    // Add services to the container.
    builder.Services.AddControllers();
    // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
    builder.Services.AddEndpointsApiExplorer();
    builder.Services.AddSwaggerGen();

    var app = builder.Build();

    var httpContextProvider = app.Services.GetRequiredService<IHttpContextAccessor>();
    HttpContextProvider.Accessor = httpContextProvider;

    app.UseCorrelationIdLogging();

    // Configure the HTTP request pipeline.
    if (app.Environment.IsDevelopment())
    {
        app.UseSwagger();
        app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "Basket API"));
    }

    app.UseHttpsRedirection();

    app.UseAuthorization();

    app.MapControllers();

    app.Run();
}
catch (Exception ex)
{
    Log.Fatal(ex, "Unhandled exception");
}
finally
{
    Log.Information("Shut down Basket API complete");
    Log.CloseAndFlush();
}