using FastEndpoints;
using Marten;
using Marten.Linq;
using Marten.Pagination;
using Me.Hypes.Helpers;
using Me.Wishlists.Helpers;
using Shared.Features.Games;
using WebAPI.Features.Auth.Extensions;
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
        Get("/search");
        Group<GamesGroupEndpoint>();
        AllowAnonymous();
    }

    public override async Task HandleAsync(GameSearchRequest req, CancellationToken ct)
    {
        await using var session = GameStore.QuerySession();
        //var gameQuery = session.Query<IGDBGame>();

        IQueryable<IGDBGame> gameQuery = session.Query<IGDBGame>();

        if (!string.IsNullOrWhiteSpace(req.Name))
        {
            gameQuery =
                gameQuery.Where(x =>
                    x.Name.Contains(req.Name, StringComparison.CurrentCultureIgnoreCase)
                );
        }

        if (req.Genres?.Length > 0)
        {
            gameQuery = gameQuery.Where(x => x.Genres != null && x.Genres.Any(g => req.Genres.Contains(g.Id)));
        }

        if (req.GameTypes?.Length > 0)
        {
            gameQuery = gameQuery.Where(x => x.GameType != null && req.GameTypes.Contains(x.GameType.Id));
        }

        if (req.Platforms?.Length > 0)
        {
            gameQuery = gameQuery.Where(x => x.Platforms != null && x.Platforms.Any(g => req.Platforms.Contains(g.Id)));
        }

        if (req.GameModes?.Length > 0)
        {
            gameQuery = gameQuery.Where(x => x.GameModes != null && x.GameModes.Any(g => req.GameModes.Contains(g.Id)));
        }

        if (req.Themes?.Length > 0)
        {
            gameQuery = gameQuery.Where(x => x.Themes != null && x.Themes.Any(g => req.Themes.Contains(g.Id)));
        }

        if (req.PlayerPerspectives?.Length > 0)
        {
            gameQuery = gameQuery.Where(x =>
                x.PlayerPerspectives != null && x.PlayerPerspectives.Any(g => req.PlayerPerspectives.Contains(g.Id)));
        }

        var games = await gameQuery.ToPagedListAsync(req.PageNumber, req.PageSize, token: ct);

        HashSet<long> hypedGames = [];
        HashSet<long> wishlistedGames = [];

        if (User.Identity.IsAuthenticated)
        {
            var userId = User.GetUserId();
            wishlistedGames = await WishlistsHelper.GetWishlistedGameIdsAsync(_context, userId, ct);
            hypedGames = await HypesHelper.GetHypedGameIdsAsync(_context, userId, ct);
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
            ),
            cancellation: ct
        );
    }
}