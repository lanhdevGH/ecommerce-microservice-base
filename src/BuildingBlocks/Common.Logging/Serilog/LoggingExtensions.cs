using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Serilog;
using Serilog.Events;
using Serilog.Filters;

namespace Common.Logging.Serilog;

public static class LoggingExtensions
{
    public static IHostBuilder UseSharedSerilog(this IHostBuilder builder, string serviceName)
    {
        builder.UseSerilog((context, loggerConfig) =>
        {
            var config = context.Configuration;
            var env = context.HostingEnvironment;

            loggerConfig
                .MinimumLevel.Information()
                .MinimumLevel.Override("Microsoft", LogEventLevel.Information)
                .MinimumLevel.Override("Microsoft.AspNetCore", LogEventLevel.Information)
                .Filter.ByExcluding(Matching.FromSource("Microsoft.AspNetCore.Mvc.Infrastructure.ControllerActionInvoker"))
                .Filter.ByExcluding(Matching.FromSource("Microsoft.AspNetCore.Routing.EndpointMiddleware"))
                .Filter.ByExcluding(Matching.FromSource("Microsoft.AspNetCore.Mvc.Infrastructure.ObjectResultExecutor"))
                .Filter.ByExcluding(Matching.FromSource("Microsoft.AspNetCore.Mvc.ModelBinding"))
                .Enrich.FromLogContext()
                .WriteTo.Console();
        });

        builder.ConfigureServices(services =>
        {
            services.AddScoped(typeof(ICustomLogger<>), typeof(SerilogCustomLogger<>));
        });

        return builder;
    }

    public static IApplicationBuilder UseCorrelationIdLogging(this IApplicationBuilder app)
    {
        return app.UseMiddleware<CorrelationIdMiddleware>();
    }
}
