using FastEndpoints;
using Marten;
using Marten.Linq;
using Marten.Pagination;
using Me.Hypes.Helpers;
using Me.Wishlists.Helpers;
using Shared.Features.Games;
using WebAPI.Features.Auth.Extensions;
using WebAPI.Features.Common;
using WebAPI.Features.Common.Endpoints;
using WebAPI.Features.Common.Games.Mappers;
using WebAPI.Features.Common.Games.Models;
using WebAPI.Features.DataAccess;

namespace Public.Games.GetGamesByParameters;

public class Endpoint : Endpoint<GameSearchRequest, PaginatedResponse<GameDto>>
{
    public IDocumentStore GameStore { get; set; }
    public LudusContext _context { get; set; }

    public override void Configure()
    {
        Post("/search");
        Group<GamesGroupEndpoint>();
        AllowAnonymous();
    }

    public override async Task HandleAsync(GameSearchRequest req, CancellationToken ct)
    {
        await using var session = GameStore.QuerySession();
        var gameQuery = session.Query<Game>();

        if (!string.IsNullOrWhiteSpace(req.Name))
        {
            gameQuery =
                (IMartenQueryable<Game>)
                    gameQuery.Where(x =>
                        x.Name.Contains(req.Name, StringComparison.CurrentCultureIgnoreCase)
                    );
        }

        if (req.GenreId is not null && req.GenreId.Length > 0)
        {
            gameQuery =
                (IMartenQueryable<Game>)
                    gameQuery.Where(x => x.Genres.Any(g => req.GenreId.Contains(g.Id)));
        }
        if (req.GameTypeId is not null && req.GameTypeId.Length > 0)
        {
            gameQuery =
                (IMartenQueryable<Game>)
                    gameQuery.Where(x => req.GameTypeId.Contains(x.GameType.Id));
        }
        if (req.PlatformId is not null && req.PlatformId.Length > 0)
        {
            gameQuery =
                (IMartenQueryable<Game>)
                    gameQuery.Where(x => x.Platforms.Any(g => req.PlatformId.Contains(g.Id)));
        }

        if (req.GameModeId is not null && req.GameModeId.Length > 0)
        {
            gameQuery =
                (IMartenQueryable<Game>)
                    gameQuery.Where(x => x.GameModes.Any(g => req.GameModeId.Contains(g.Id)));
        }

        if (req.ThemeId is not null && req.ThemeId.Length > 0)
        {
            gameQuery =
                (IMartenQueryable<Game>)
                    gameQuery.Where(x => x.Themes.Any(g => req.ThemeId.Contains(g.Id)));
        }

        if (req.PlayerPerspectiveId is not null && req.PlayerPerspectiveId.Length > 0)
        {
            gameQuery =
                (IMartenQueryable<Game>)
                    gameQuery.Where(x =>
                        x.PlayerPerspectives.Any(g => req.PlayerPerspectiveId.Contains(g.Id))
                    );
        }

        var games = await gameQuery.ToPagedListAsync(req.PageNumber, req.PageSize);

        HashSet<long> hypedGames = [];
        HashSet<long> wishlistedGames = [];

        if (User.Identity.IsAuthenticated)
        {
            var userId = User.GetUserId();
            wishlistedGames = await WishlistsHelper.GetWishlistedGameIdsAsync(_context, userId);
            hypedGames = await HypesHelper.GetHypedGameIdsAsync(_context, userId);
        }
        var previews = GameDtoMapper.MapGamesToDto(games, wishlistedGames, hypedGames);

        await SendAsync(
            new PaginatedResponse<GameDto>(
                previews,
                games.TotalItemCount,
                games.PageCount,
                games.PageSize,
                games.PageNumber,
                games.IsLastPage
            )
        );
    }
}
