using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Hosting;
using Serilog;
using Serilog.Events;

namespace Common.Logging;

public static class LoggingExtensions
{
    public static IHostBuilder UseSharedSerilog(this IHostBuilder builder, string serviceName)
    {
        return builder.UseSerilog((context, loggerConfig) =>
        {
            var config = context.Configuration;
            var env = context.HostingEnvironment;

            loggerConfig
                .MinimumLevel.Debug()
                .MinimumLevel.Override("Microsoft", LogEventLevel.Information)
                .WriteTo.Console();
        });
    }

    public static IApplicationBuilder UseCorrelationIdLogging(this IApplicationBuilder app)
    {
        return app.UseMiddleware<CorrelationIdMiddleware>();
    }
}
