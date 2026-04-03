using System.Globalization;
using System.Threading.RateLimiting;
using Microsoft.AspNetCore.RateLimiting;

namespace PlayAPI.Extensions;

public static class RateLimitingExtensions
{
    public const string GameVisitsPolicyName = "game-visits";

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
                            PermitLimit = 240,
                            Window = TimeSpan.FromMinutes(1),
                            QueueLimit = 0,
                            AutoReplenishment = true,
                        }
                    );
                }
            );

            options.AddPolicy<string>(
                GameVisitsPolicyName,
                httpContext =>
                {
                    var ipAddress = httpContext.Connection.RemoteIpAddress?.ToString() ?? "unknown";
                    var gameId = httpContext.Request.RouteValues["gameId"]?.ToString() ?? "unknown";
                    var currentDay = DateOnly.FromDateTime(DateTime.UtcNow).ToString("yyyy-MM-dd");

                    return RateLimitPartition.GetFixedWindowLimiter(
                        partitionKey: $"{ipAddress}:{gameId}:{currentDay}",
                        factory: _ => new FixedWindowRateLimiterOptions
                        {
                            PermitLimit = 1,
                            Window = TimeSpan.FromDays(1),
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
