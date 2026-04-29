using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using PlayAPI.Context;
using PlayAPI.Features.Cookies;

namespace PlayAPI.Features.Games.Analytics.GameMetrics;

public static class GetGameMetricEndpoints
{
    public sealed record GetGameMetricResponse(
        [Required] long GameId,
        [Required] Guid SessionId,
        [Required] DateTime FirstVisitedAt,
        [Required] DateTime LastVisitedAt,
        [Required] int ViewCount
    );

    public sealed record GetGameStatsResponse(
        [Required] long GameId,
        [Required] DateTime FirstVisitedAt,
        [Required] DateTime LastVisitedAt,
        [Required] int TotalViewCount,
        [Required] int SessionCount
    );

    public static IEndpointRouteBuilder MapGetGameMetricEndpoints(this IEndpointRouteBuilder app)
    {
        app.MapGet("/play/game-metrics", HandleGameMetricAsync)
            .AddEndpointFilter<ConsentFilter>()
            .WithName("GamesAnalytics/GetGameMetric")
            .Produces<GetGameMetricResponse>(StatusCodes.Status200OK)
            .Produces(StatusCodes.Status404NotFound)
            .Produces(StatusCodes.Status204NoContent)
            .ProducesValidationProblem(StatusCodes.Status400BadRequest);

        app.MapGet("/play/game-stats", HandleGameStatsAsync)
            .WithName("GamesAnalytics/GetGameStats")
            .Produces<GetGameStatsResponse>(StatusCodes.Status200OK)
            .Produces(StatusCodes.Status404NotFound)
            .ProducesValidationProblem(StatusCodes.Status400BadRequest);

        return app;
    }

    private static async Task<IResult> HandleGameMetricAsync(
        long gameId,
        ICookieService cookieService,
        HttpContext httpContext,
        AppDbContext db,
        CancellationToken cancellationToken
    )
    {
        var validationErrors = new Dictionary<string, string[]>();

        if (gameId <= 0)
        {
            validationErrors[nameof(gameId)] = ["GameId must be greater than 0."];
        }

        var sessionId = cookieService.GetSessionId(httpContext);
        if (!sessionId.HasValue)
        {
            return Results.NoContent();
        }

        var stats = await db
            .GameMetrics.AsNoTracking()
            .FirstOrDefaultAsync(
                s => s.GameId == gameId && s.SessionId == sessionId.GetValueOrDefault(),
                cancellationToken
            );

        if (stats is null)
            return Results.NotFound();

        return Results.Ok(
            new GetGameMetricResponse(
                stats.GameId,
                stats.SessionId,
                stats.FirstVisitedAt.ToDateTimeUtc(),
                stats.LastVisitedAt.ToDateTimeUtc(),
                stats.ViewCount
            )
        );
    }

    private static async Task<IResult> HandleGameStatsAsync(
        long gameId,
        AppDbContext db,
        CancellationToken cancellationToken
    )
    {
        var validationErrors = new Dictionary<string, string[]>();

        if (gameId <= 0)
        {
            validationErrors[nameof(gameId)] = ["GameId must be greater than 0."];
        }

        if (validationErrors.Count > 0)
        {
            return Results.ValidationProblem(validationErrors);
        }

        var stats = await db
            .GameMetrics.AsNoTracking()
            .Where(s => s.GameId == gameId)
            .GroupBy(s => s.GameId)
            .Select(g => new
            {
                GameId = g.Key,
                FirstVisitedAt = g.Min(s => s.FirstVisitedAt),
                LastVisitedAt = g.Max(s => s.LastVisitedAt),
                TotalViewCount = g.Sum(s => s.ViewCount),
                SessionCount = g.Count(),
            })
            .FirstOrDefaultAsync(cancellationToken);

        if (stats is null)
            return Results.NotFound();

        return Results.Ok(
            new GetGameStatsResponse(
                stats.GameId,
                stats.FirstVisitedAt.ToDateTimeUtc(),
                stats.LastVisitedAt.ToDateTimeUtc(),
                stats.TotalViewCount,
                stats.SessionCount
            )
        );
    }
}
