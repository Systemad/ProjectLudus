using CatalogAPI.Features.Companies;
using CatalogAPI.Features.GameEngines;
using CatalogAPI.Features.Games;
using CatalogAPI.Features.Platforms;
using CatalogAPI.Features.References.Genres;
using CatalogAPI.Features.References.Themes;
using IGDB.Models;
using Shared.Features;

namespace CatalogAPI.Features;

public static class QueryHelper
{
    public static Dictionary<IgdbType, (string Endpoint, List<string> Fields)> QueryMaps =
        new()
        {
            { IgdbType.PLATFORM, (PlatformQuery.Endpoint, PlatformQuery.Fields) },
            { IgdbType.GAME_ENGINE, (GameEngineQuery.Endpoint, GameEngineQuery.Fields) },
            { IgdbType.COMPANY, (CompanyQuery.Endpoint, CompanyQuery.Fields) },
            { IgdbType.THEME, (ThemeQuery.Endpoint, ThemeQuery.Fields) },
            { IgdbType.GENRE, (GenreQuery.Endpoint, GenreQuery.Fields) },
            { IgdbType.GAME, (GameQuery.Endpoint, GameQuery.Fields) },
        };

}
