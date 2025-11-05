using Catalog.Worker.Features.Companies;
using CatalogAPI.Features.GameEngines;
using CatalogAPI.Features.Games;
using CatalogAPI.Features.Genres;
using CatalogAPI.Features.Platforms;
using CatalogAPI.Features.PlayerPerspective;
using CatalogAPI.Features.Themes;
using Shared.Features;

namespace Catalog.Worker.Queries;

public static class QueryHelper
{
    public static Dictionary<IgdbType, (string Endpoint, List<string> Fields)> QueryMaps = new()
    {
        // Done
        { IgdbType.PLATFORM, (PlatformQuery.Endpoint, PlatformQuery.Fields) },
        { IgdbType.GENRE, (GenreQuery.Endpoint, GenreQuery.Fields) },
        { IgdbType.THEME, (ThemeQuery.Endpoint, ThemeQuery.Fields) },
        {
            IgdbType.PlayerPerspective,
            (PlayerPerspectiveQuery.Endpoint, PlayerPerspectiveQuery.Fields)
        },
        // Not Done
        { IgdbType.GAME_ENGINE, (GameEngineQuery.Endpoint, GameEngineQuery.Fields) },
        { IgdbType.COMPANY, (CompanyQuery.Endpoint, CompanyQuery.Fields) },
        { IgdbType.GAME, (GameQuery.Endpoint, GameQuery.Fields) },
    };
}
