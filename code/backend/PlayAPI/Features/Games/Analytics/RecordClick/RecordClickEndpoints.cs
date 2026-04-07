using PlayAPI.Extensions;

namespace PlayAPI.Features.Games.Analytics.RecordClick;

public static class RecordClickEndpoints
{
    public sealed record RecordGameClickResponse(long GameId, long Count, DateTime LastVisitedAt);

    public static IEndpointRouteBuilder MapRecordClickEndpoints(
        this IEndpointRouteBuilder routeBuilder
    )
    {
        var group = routeBuilder.MapGroup("/api/games");

        group
            .MapPost("/{gameId:long}/clicks", RecordGameClickAsync)
            .Produces<RecordGameClickResponse>(StatusCodes.Status200OK)
            .Produces(StatusCodes.Status404NotFound)
            .RequireRateLimiting(RateLimitingExtensions.GameVisitsPolicyName);

        return routeBuilder;
    }

    private static async Task<IResult> RecordGameClickAsync(
        long gameId,
        GameClickTrackingService trackingService,
        CancellationToken cancellationToken
    )
    {
        if (gameId <= 0)
        {
            return TypedResults.NotFound();
        }

        var click = await trackingService.RecordClickAsync(gameId, cancellationToken);

        return TypedResults.Ok(
            new RecordGameClickResponse(click.GameId, click.Count, click.LastVisitedAt)
        );
    }
}
