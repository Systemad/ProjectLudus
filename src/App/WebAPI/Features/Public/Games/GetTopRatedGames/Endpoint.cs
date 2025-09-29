using FastEndpoints;
using Marten;
using Marten.Pagination;
using Shared.Features;
using Shared.Features.Games;
using Shared.Features.References.Platform;
using WebAPI.Features.Common.Endpoints;
using WebAPI.Features.Common.Games.Mappers;
using WebAPI.Features.Common.Games.Models;

namespace Public.Games.GetTopRatedGames;

public class Endpoint : Endpoint<GetTopRatedGamesRequest, PaginatedResponse<GameDto>>
{
    public IDocumentStore Store { get; set; }
    public override void Configure()
    {
        Get("/top");
        Group<GamesGroupEndpoint>();
        AllowAnonymous();
    }

    public override async Task HandleAsync(GetTopRatedGamesRequest req, CancellationToken ct)
    {
        await using var session = Store.LightweightSession();

        var platformDict = new Dictionary<long, Platform>();
        var games = await session
            .Query<IGDBGameFlat>()
            .Include(platformDict).On(x => x.Platforms)
            .Where(x => x.GameType.Id == 0 && x.TotalRatingCount >= 90)
            .OrderByDescending(x => x.TotalRating)
            .ThenByDescending(x => x.TotalRatingCount)
            .ToPagedListAsync(req.PageNumber, req.PageSize, token: ct);

        var prev = games.Select(item =>
                item.MapToGameDto(
                    platformDict))
            .ToList();

        await Send.OkAsync(
            new PaginatedResponse<GameDto>(
                prev,
                games.TotalItemCount,
                games.PageCount,
                games.PageSize,
                games.PageNumber,
                games.IsLastPage
            ),
            cancellation: ct
        );
    }
}