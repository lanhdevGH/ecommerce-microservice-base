using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Ordering.Infrastructure.Persistence;

namespace Ordering.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructureServices(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddDbContext<OrderDBContext>(options =>
        {
            options.UseSqlServer(configuration.GetConnectionString("OrderingConnection"),
                builder => builder.MigrationsAssembly(typeof(OrderDBContext).Assembly.FullName));
        });

        services.AddScoped<OrderDBContextSeed>();

        return services;
    }
}
