using FastEndpoints;
using Marten.Linq;
using Marten.Pagination;
using Shared.Features.Games;
using WebAPI.Features.Common;
using WebAPI.Features.Common.Games.Services;

namespace Public.Games.GetGamesByParameters;

public class Endpoint : Endpoint<GameSearchRequest, GetSearchGamesResponse>
{
    public IGameStore GameStore { get; set; }
    public IGameService GameService { get; set; }

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
        var previews = await GameService.CreateGameDtoAsync(User, games.Select(x => x.Id));
        await SendOkAsync(
            new GetSearchGamesResponse(
                previews,
                games.TotalItemCount,
                games.PageCount,
                games.PageNumber,
                games.IsLastPage
            )
        );
    }
}
