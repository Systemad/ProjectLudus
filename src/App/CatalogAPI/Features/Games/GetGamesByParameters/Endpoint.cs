using CatalogAPI.Common.Endpoints;
using CatalogAPI.Data;
using CatalogAPI.Data.Features.Games;
using FastEndpoints;

namespace Features.Games.GetGamesByParameters;

public class Endpoint : Endpoint<GameSearchRequest, PaginatedResponse<GameDto>>
{
    public CatalogContext Context { get; set; }

    public override void Configure()
    {
        Get("/search");
        Group<GamesGroupEndpoint>();
        AllowAnonymous();
    }

    public override async Task HandleAsync(GameSearchRequest req, CancellationToken ct)
    {
        IQueryable<GameEntity> gameQuery = Context.Games;

        if (!string.IsNullOrWhiteSpace(req.Query))
        {
            /*
            gameQuery =
                gameQuery.Where(x =>
                    x.SearchField.NgramSearch(req.Query)
                );
            */
        }

        if (req.Genres?.Length > 0)
        {
            var genreIds = req.Genres;
            gameQuery = gameQuery.Where(game =>
                game.Genres != null && game.Genres.Any(g => genreIds.Contains(g.Id))
            );

            //gameQuery = gameQuery.Where(x => x.Genres != null && x.Genres.Any(g => req.Genres.Contains(g.Id)));
        }

        if (req.GameTypes?.Length > 0)
        {
            var gameTypeFilter = req.GameTypes;
            gameQuery = gameQuery.Where(x =>
                x.GameType != null && gameTypeFilter.Contains(x.GameType)
            );
        }

        if (req.Platforms?.Length > 0)
        {
            var platformIds = req.Platforms;
            gameQuery = gameQuery.Where(game =>
                game.Platforms != null && game.Platforms.Any(g => platformIds.Contains(g.Id))
            );

            //gameQuery = gameQuery.Where(x => x.Platforms != null && x.Platforms.Any(g => req.Platforms.Contains(g)));
        }

        if (req.GameModes?.Length > 0)
        {
            var gameModeIds = req.GameModes;
            gameQuery = gameQuery.Where(game =>
                game.GameModes != null && game.GameModes.Any(g => gameModeIds.Contains(g.Id))
            );

            //gameQuery = gameQuery.Where(x => x.GameModes != null && x.GameModes.Any(g => req.GameModes.Contains(g)));
        }

        if (req.Themes?.Length > 0)
        {
            var themeIds = req.Themes;
            gameQuery = gameQuery.Where(game =>
                game.Themes != null && game.Themes.Any(g => themeIds.Contains(g.Id))
            );
            //gameQuery = gameQuery.Where(x => x.Themes != null && x.Themes.Any(g => req.Themes.Contains(g)));
        }

        // TODO: ADD!
        /*
        if (req.PlayerPerspectives?.Length > 0)
        {
            var ppsId = req.PlayerPerspectives;
            gameQuery = gameQuery.Where(game =>
                game.GameModes != null && game.GameModes.Any(g => gameModeIds.Contains(g.Id))
            );
            //gameQuery = gameQuery.Where(x => x.PlayerPerspectives.Any(g => req.PlayerPerspectives.Contains(g)));
        }
        */

        var games = new List<GameDto>();

        await Send.OkAsync(
            new PaginatedResponse<GameDto>(Items: games, 1, 1, 1, 1, false),
            cancellation: ct
        );
    }
}
