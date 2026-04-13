using Microsoft.AspNetCore.OutputCaching;
using NodaTime.Text;

namespace CatalogAPI.Features.PopularityTypes.GetById;

public static class GetByIdEndpoints
{
    private record GetPopTypesQuery(long PopularityTypeId, int Limit = 20, string? Date = null);

    private record PopularityGamesResponse(List<GamesSearchDto> Games);

    public static IEndpointRouteBuilder MapGetByIdEndpoints(this IEndpointRouteBuilder routeBuilder)
    {
        var group = routeBuilder.MapGroup("/api/popularity").CacheOutput("DefaultCache");

        group
            .MapGet("/{popularityTypeId:long}", GetPopularityRailAsync)
            .Produces<PopularityGamesResponse>(StatusCodes.Status200OK);

        return routeBuilder;
    }

    private static async Task<IResult> GetPopularityRailAsync(
        [AsParameters] GetPopTypesQuery request,
        AppDbContext db,
        CancellationToken cancellationToken
    )
    {
        var latestDate = await db
            .PopularityPrimitives.Where(p => p.PopularityType == request.PopularityTypeId)
            .MaxAsync(p => p.SnapshotDate, cancellationToken);

        if (latestDate is null)
            return Results.Ok(new PopularityGamesResponse([]));

        LocalDate targetDate;
        if (!string.IsNullOrWhiteSpace(request.Date) && request.Date != "today")
        {
            var parseResult = LocalDatePattern.Iso.Parse(request.Date!);
            if (!parseResult.Success)
                return Results.BadRequest("Invalid date format. Use yyyy-MM-dd");

            targetDate = parseResult.Value;
            if (targetDate > latestDate.Value)
                return Results.Ok(new PopularityGamesResponse([]));
        }
        else
        {
            targetDate = latestDate.Value;
        }

        var topGames = await db
            .PopularityPrimitives.Where(p =>
                p.PopularityType == request.PopularityTypeId
                && p.SnapshotDate == targetDate
                && p.GameId.HasValue
            )
            .GroupBy(p => p.GameId!.Value)
            .Select(g => new { GameId = g.Key, MaxScore = g.Max(p => p.Value) })
            .Join(
                db.GamesSearches,
                top => top.GameId,
                g => g.Id,
                (top, g) => new { Game = g, top.MaxScore }
            )
            .OrderByDescending(x => x.MaxScore)
            .Take(request.Limit)
            .Select(x => x.Game)
            .ToListAsync(cancellationToken);

        return Results.Ok(new PopularityGamesResponse(GameSearchMapper.MapToDto(topGames)));
    }
}
