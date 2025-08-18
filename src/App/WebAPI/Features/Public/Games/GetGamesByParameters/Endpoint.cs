using FastEndpoints;
using Marten;
using Marten.Pagination;
using Me.Hypes.Helpers;
using Me.Wishlists.Helpers;
using Shared.Features;
using Shared.Features.Games;
using WebAPI.Features.Auth.Extensions;
using WebAPI.Features.Common.Endpoints;
using WebAPI.Features.Common.Games.Mappers;
using WebAPI.Features.Common.Games.Models;
using WebAPI.Features.DataAccess;

namespace Public.Games.GetGamesByParameters;

public class Endpoint : Endpoint<GameSearchRequest, PaginatedResponse<GamePreviewDto>>
{
    public IDocumentStore GameStore { get; set; }
    public LudusContext _context { get; set; }

    public override void Configure()
    {
        Get("/search");
        Group<GamesGroupEndpoint>();
        AllowAnonymous();
    }

    public override async Task HandleAsync(GameSearchRequest req, CancellationToken ct)
    {
        await using var session = GameStore.LightweightSession();

        IQueryable<IGDBGameFlat> gameQuery = session.Query<IGDBGameFlat>();

        if (!string.IsNullOrWhiteSpace(req.Name))
        {
            gameQuery =
                gameQuery.Where(x =>
                    x.WebStyleSearch(req.Name)
                );
        }

        if (req.Genres?.Length > 0)
        {
            gameQuery = gameQuery.Where(x => x.Genres != null && x.Genres.Any(g => req.Genres.Contains(g)));
        }

        if (req.GameTypes?.Length > 0)
        {
            gameQuery = gameQuery.Where(x => x.GameType != null && req.GameTypes.Contains(x.GameType.Id));
        }

        if (req.Platforms?.Length > 0)
        {
            gameQuery = gameQuery.Where(x => x.Platforms != null && x.Platforms.Any(g => req.Platforms.Contains(g)));
        }

        if (req.GameModes?.Length > 0)
        {
            gameQuery = gameQuery.Where(x => x.GameModes != null && x.GameModes.Any(g => req.GameModes.Contains(g)));
        }

        if (req.Themes?.Length > 0)
        {
            gameQuery = gameQuery.Where(x => x.Themes != null && x.Themes.Any(g => req.Themes.Contains(g)));
        }

        if (req.PlayerPerspectives?.Length > 0)
        {
            gameQuery = gameQuery.Where(x => x.PlayerPerspectives.Any(g => req.PlayerPerspectives.Contains(g)));
        }

        var platformDict = new Dictionary<long, Platform>();
        var games =
            await gameQuery
                .Where(x => x.GameType.Id == 0)
                .Include(platformDict).On(x => x.Platforms)
                .OrderBySql(Sorting.SortFilerOne)
                .ToPagedListAsync(req.PageNumber, req.PageSize, token: ct);

        HashSet<long> hypedGames = [];
        HashSet<long> wishlistedGames = [];

        if (User.Identity.IsAuthenticated)
        {
            var userId = User.GetUserId();
            wishlistedGames = await WishlistsHelper.GetWishlistedGameIdsAsync(_context, userId, ct);
            hypedGames = await HypesHelper.GetHypedGameIdsAsync(_context, userId, ct);
        }

        var previews = games.Select(item =>
                item.ToGamePreviewDto(
                    platformDict,
                    wishlistedGames.Contains(item.Id),
                    hypedGames.Contains(item.Id)))
            .ToList();

        await Send.OkAsync(
            new PaginatedResponse<GamePreviewDto>(
                previews,
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