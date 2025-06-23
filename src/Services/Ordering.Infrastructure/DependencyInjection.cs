using Contracts.Common.Interfaces;
using Contracts.Services.SMTP;
using Infrastructure.Configurations;
using Infrastructure.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Ordering.Application.Common.Interfaces;
using Ordering.Infrastructure.Persistence;
using Ordering.Infrastructure.Repositories;

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

        var emailSettings = configuration.GetSection(nameof(SMTPEmailSettings))
            .Get<SMTPEmailSettings>();

        if (emailSettings != null)
        {
            services.AddSingleton(emailSettings);
        }

        services.AddScoped<OrderDBContextSeed>();
        services.AddScoped(typeof(IRepositoryBase<,>), typeof(RepositoryBase<,>));
        services.AddScoped<IUnitOfWork, UnitOfWork>();
        services.AddScoped<IOrderRepository, OrderRepository>();
        services.AddScoped(typeof(ISmtpEmailService), typeof(SmtpEmailService));
        return services;
    }
}
