using Basket.API.Repositories.Interfaces;
using Basket.API.Repositories;
using Contracts.Common.Interfaces;
using Infrastructure.Common;

namespace Basket.API.Extensions;

public static class ServiceExtensions
{
    public static void ConfigureRedis(this IServiceCollection services, IConfiguration configuration)
    {
        var redisConnectionString = configuration.GetSection("CacheSettings:ConnectionString").Value;
        if (string.IsNullOrEmpty(redisConnectionString))
            throw new ArgumentException("Redis Connection string is not configured!");
        // Redis Configuration
        services.AddStackExchangeRedisCache(options =>
        {
            options.Configuration = redisConnectionString;
            options.InstanceName = "BasketService_";
        });
    }

    public static IServiceCollection ConfigureServices(this IServiceCollection services)
    {
        services.AddScoped<IBasketRepository, BasketRepository>();
        services.AddTransient<ISerializerService, SerializerService>();
        return services;
    }
}
