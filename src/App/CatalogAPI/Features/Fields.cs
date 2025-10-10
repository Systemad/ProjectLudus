using CatalogAPI.Features.Companies;
using CatalogAPI.Features.GameEngines;
using CatalogAPI.Features.Games;
using CatalogAPI.Features.Platforms;
using CatalogAPI.Features.References.Genres;
using CatalogAPI.Features.References.Themes;
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
