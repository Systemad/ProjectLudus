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

public class Endpoint : Endpoint<GameSearchRequest, PaginatedResponse<GameDto>>
{
    public IDocumentStore GameStore { get; set; }
    public LudusContext _context { get; set; }
    public GameDtoMapper Mapper { get; set; }

    public override void Configure()
    {
        Get("/search");
        Group<GamesGroupEndpoint>();
        AllowAnonymous();
    }

    public override async Task HandleAsync(GameSearchRequest req, CancellationToken ct)
    {
        await using var session = GameStore.LightweightSession();

        IQueryable<InsertIGDBGame> gameQuery = session.Query<InsertIGDBGame>();

        if (!string.IsNullOrWhiteSpace(req.Name))
        {
            gameQuery =
                gameQuery.Where(x =>
                    x.Name.Contains(req.Name, StringComparison.CurrentCultureIgnoreCase)
                );
        }

        if (req.Genres?.Count > 0)
        {
            gameQuery = gameQuery.Where(x => x.Genres != null && x.Genres.Any(g => req.Genres.Contains(g)));
        }

        if (req.GameTypes?.Count > 0)
        {
            gameQuery = gameQuery.Where(x => x.GameType != null && req.GameTypes.Contains(x.GameType.Id));
        }

        if (req.Platforms?.Count > 0)
        {
            gameQuery = gameQuery.Where(x => x.Platforms != null && x.Platforms.Any(g => req.Platforms.Contains(g)));
        }

        if (req.GameModes?.Count > 0)
        {
            gameQuery = gameQuery.Where(x => x.GameModes != null && x.GameModes.Any(g => req.GameModes.Contains(g)));
        }

        if (req.Themes?.Count > 0)
        {
            gameQuery = gameQuery.Where(x => x.Themes != null && x.Themes.Any(g => req.Themes.Contains(g)));
        }

        if (req.PlayerPerspectives?.Count > 0)
        {
            gameQuery = gameQuery.Where(x =>
                x.PlayerPerspectives != null && x.PlayerPerspectives.Any(g => req.PlayerPerspectives.Contains(g)));
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

        var previews = Mapper.MapGamesToDto(games, wishlistedGames, hypedGames);

        await Send.OkAsync(
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