using Common.Logging;
using Contracts.Common.Interfaces;
using Customer.API;
using Customer.API.Endpoints;
using Customer.API.Persistence;
using Customer.API.Repositories;
using Customer.API.Repositories.Interfaces;
using Customer.API.Services;
using Customer.API.Services.Interfaces;
using Infrastructure.Common;
using Microsoft.EntityFrameworkCore;
using Serilog;

var builder = WebApplication.CreateBuilder(args);

Log.Information($"Start {builder.Environment.ApplicationName} up");

try
{
    builder.Host.UseSharedSerilog("Customer service");
    builder.Services.AddHttpContextAccessor();
    builder.Services.AddControllers();
    builder.Services.AddAutoMapper(cfg => cfg.AddProfile(new MappingProfile()));
    // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
    builder.Services.AddEndpointsApiExplorer();
    builder.Services.AddSwaggerGen();

    var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
    builder.Services.AddDbContext<CustomerContext>(
        options => options.UseNpgsql(connectionString));
    builder.Services.AddScoped<ICustomerRepository, CustomerRepository>()
        .AddScoped(typeof(IRepositoryBase<,,>), typeof(RepositoryBase<,,>))
        .AddScoped(typeof(IUnitOfWork<>), typeof(UnitOfWork<>))
        .AddScoped<ICustomerService, CustomerService>();

    var app = builder.Build();

    var httpContextProvider = app.Services.GetRequiredService<IHttpContextAccessor>();
    HttpContextProvider.Accessor = httpContextProvider;

    app.UseCorrelationIdLogging();
    // Configure the HTTP request pipeline.
    app.MapGet("/", () => $"Welcome to {builder.Environment.ApplicationName}!");
    app.MapCustomerEndpoints();

    app.UseSwagger();

    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json",
            $"Swagger {builder.Environment.ApplicationName} v1");
    });

    // app.UseHttpsRedirection(); //production only

    app.UseAuthorization();

    app.MapControllers();

    app.SeedCustomerData()
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
    Log.Information($"Shut down {builder.Environment.ApplicationName} complete");
    Log.CloseAndFlush();
}