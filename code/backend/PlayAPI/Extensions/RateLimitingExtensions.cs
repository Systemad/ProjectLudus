using System.Globalization;
using System.Threading.RateLimiting;
using Microsoft.AspNetCore.RateLimiting;

namespace PlayAPI.Extensions;

public static class RateLimitingExtensions
{
    public static IServiceCollection AddPlayApiRateLimiting(this IServiceCollection services)
    {
        services.AddRateLimiter(options =>
        {
            options.OnRejected = async (context, cancellationToken) =>
            {
                if (context.Lease.TryGetMetadata(MetadataName.RetryAfter, out var retryAfter))
                {
                    var retryAfterSeconds = ((int)retryAfter.TotalSeconds).ToString(
                        NumberFormatInfo.InvariantInfo
                    );
                    context.HttpContext.Response.Headers.RetryAfter = retryAfterSeconds;
                }

                context.HttpContext.Response.StatusCode = StatusCodes.Status429TooManyRequests;
                await context.HttpContext.Response.WriteAsync(
                    "Too many requests. Please try again later.",
                    cancellationToken
                );
            };

            options.GlobalLimiter = PartitionedRateLimiter.Create<HttpContext, string>(
                httpContext =>
                {
                    var ipAddress = httpContext.Connection.RemoteIpAddress?.ToString() ?? "unknown";

                    return RateLimitPartition.GetFixedWindowLimiter(
                        partitionKey: ipAddress,
                        factory: _ => new FixedWindowRateLimiterOptions
                        {
                            PermitLimit = 100,
                            Window = TimeSpan.FromMinutes(1),
                            QueueLimit = 0,
                            AutoReplenishment = true,
                        }
                    );
                }
            );
        });

        return services;
    }
}
