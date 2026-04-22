using CatalogAPI.Features;
using Microsoft.AspNetCore.OutputCaching;
using Microsoft.EntityFrameworkCore;

namespace CatalogAPI.Features.Stats.GetStats;

public static class GetStatsEndpoint
{
    public sealed record GetStatsResponse(
        long TotalGames,
        long TotalCompanies,
        long TotalPlatforms,
        long TotalEvents
    );

    public static RouteHandlerBuilder MapGetStatsEndpoint(
        this IEndpointRouteBuilder routeBuilder
    )
    {
        return routeBuilder
            .MapGet("/", GetStatsAsync)
            .WithName($"{EndpointMetadata.Stats}/GetStats")
            .WithTags(EndpointMetadata.Stats)
            .Produces<GetStatsResponse>(StatusCodes.Status200OK);
    }

    private static async Task<IResult> GetStatsAsync(
        AppDbContext db,
        CancellationToken cancellationToken
    )
    {
        var totalGames = await db.Games.LongCountAsync(cancellationToken);
        var totalCompanies = await db.Companies.LongCountAsync(cancellationToken);
        var totalPlatforms = await db.Platforms.LongCountAsync(cancellationToken);
        var totalEvents = await db.Events.LongCountAsync(cancellationToken);

        return Results.Ok(
            new GetStatsResponse(
                TotalGames: totalGames,
                TotalCompanies: totalCompanies,
                TotalPlatforms: totalPlatforms,
                TotalEvents: totalEvents
            )
        );
    }
}
