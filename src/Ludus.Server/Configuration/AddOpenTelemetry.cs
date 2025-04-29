using OpenTelemetry;
using OpenTelemetry.Metrics;
using OpenTelemetry.Trace;

namespace Ludus.Server.Configuration;

public static class AddOpenTelemetry
{
    public static IServiceCollection ConfigureOpenTelemetry(
        this IServiceCollection services,
        IConfiguration configuration
    )
    {
        services
            .AddOpenTelemetry()
            .WithTracing(tracing =>
            {
                tracing.AddHttpClientInstrumentation();
                tracing.AddSource("Marten");
            })
            .WithMetrics(metrics =>
            {
                metrics.AddHttpClientInstrumentation().AddRuntimeInstrumentation();
                metrics.AddMeter("Marten");
            });
        var useOtlpExporter = !string.IsNullOrWhiteSpace(
            configuration["OTEL_EXPORTER_OTLP_ENDPOINT"]
        );
        if (useOtlpExporter)
        {
            services.AddOpenTelemetry().UseOtlpExporter();
        }

        return services;
    }
}
