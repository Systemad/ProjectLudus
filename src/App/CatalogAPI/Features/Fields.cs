using CatalogAPI.Features.Companies;
using CatalogAPI.Features.GameEngines;
using CatalogAPI.Features.Games;
using CatalogAPI.Features.Genres;
using CatalogAPI.Features.Platforms;
using CatalogAPI.Features.Themes;
using Shared.Features;

namespace CatalogAPI.Features;

public static class Endpoints
{
    public static Dictionary<IgdbReference, (string Endpoint, List<string> Fields)> QueryMaps =
        new()
        {
            { IgdbReference.PLATFORM, (PlatformQuery.Endpoint, PlatformQuery.Fields) },
            { IgdbReference.GAME_ENGINE, (GameEngineQuery.Endpoint, GameEngineQuery.Fields) },
            { IgdbReference.COMPANY, (CompanyQuery.Endpoint, CompanyQuery.Fields) },
            { IgdbReference.THEME, (ThemeQuery.Endpoint, ThemeQuery.Fields) },
            { IgdbReference.GENRE, (GenreQuery.Endpoint, GenreQuery.Fields) },
            { IgdbReference.GAME, (GameQuery.Endpoint, GameQuery.Fields) },
        };
}
