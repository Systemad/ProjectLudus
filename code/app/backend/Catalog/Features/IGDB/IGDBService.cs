using Catalog.Features.Games.Common.Pagination;
using Catalog.Features.Games.Common.Projections;
using Catalog.Features.IGDB.GetStatistics;

namespace Catalog.Features.IGDB;

internal sealed class IGDBService(AppDbContext db) : IIGDBService
{
    public async Task<PagedGamesResponse> GetMostAnticipatedAsync(
        int page,
        int pageSize,
        CancellationToken ct
    )
    {
        var now = DateTime.UtcNow;
        var oneMonthFromNow = now.AddDays(30);
        var skip = (page - 1) * pageSize;

        var games = await db
            .Games.Where(game =>
                game.FirstReleaseDateUtc >= now && game.FirstReleaseDateUtc < oneMonthFromNow
            )
            .Where(game =>
                game.GameTypeNavigation != null && game.GameTypeNavigation.Type == "Main Game"
            )
            .OrderByDescending(game => game.Hypes)
            .ThenByDescending(game => game.TotalRatingCount)
            .ThenBy(game => game.Id)
            .Skip(skip)
            .Take(pageSize + 1)
            .SelectGameBrowseDto()
            .ToListAsync(ct);

        return PagedGamesResponse.Create(games, page, pageSize);
    }

    public async Task<PagedGamesResponse> GetPopscoreAsync(
        long popularityTypeId,
        DateTime? from,
        DateTime? to,
        int page,
        int pageSize,
        CancellationToken ct
    )
    {
        var duplicateGameId = await db
            .Games.Where(game =>
                game.PopularityPrimitives.Count(popularity =>
                    popularity.PopularityType == popularityTypeId
                ) > 1
            )
            .Select(game => (long?)game.Id)
            .FirstOrDefaultAsync(ct);

        if (duplicateGameId.HasValue)
        {
            throw new InvalidOperationException(
                $"Game {duplicateGameId.Value} has multiple popularity primitives for type {popularityTypeId}."
            );
        }

        var query = db
            .Games.Where(game =>
                game.PopularityPrimitives.Any(popularity =>
                    popularity.PopularityType == popularityTypeId
                )
            )
            .Select(game => new
            {
                Game = game,
                Value = game
                    .PopularityPrimitives.Where(popularity =>
                        popularity.PopularityType == popularityTypeId
                    )
                    .Select(popularity => popularity.Value)
                    .Single(),
            });

        if (from.HasValue)
        {
            query = query.Where(item => item.Game.FirstReleaseDateUtc >= from.Value);
        }

        if (to.HasValue)
        {
            query = query.Where(item => item.Game.FirstReleaseDateUtc < to.Value);
        }

        var skip = (page - 1) * pageSize;

        var games = await query
            .OrderByDescending(item => item.Value)
            .ThenBy(item => item.Game.Id)
            .Select(item => item.Game)
            .Skip(skip)
            .Take(pageSize + 1)
            .SelectGameBrowseDto()
            .ToListAsync(ct);

        return PagedGamesResponse.Create(games, page, pageSize);
    }

    public async Task<StatisticsResponse> GetStatisticsAsync(CancellationToken ct)
    {
        var totalGames = await db.Games.LongCountAsync(ct);
        var totalCompanies = await db.Companies.LongCountAsync(ct);
        var totalPlatforms = await db.Platforms.LongCountAsync(ct);
        var totalEvents = await db.Events.LongCountAsync(ct);

        return new StatisticsResponse(totalGames, totalCompanies, totalPlatforms, totalEvents);
    }
}
