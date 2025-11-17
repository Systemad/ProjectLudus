using BuildingBlocks.Twitch;

namespace Catalog.Worker.Queries;

public static class QueryHelper
{
    public static Dictionary<IgdbType, (string Endpoint, List<string> Fields)> QueryMaps = new()
    {
        { IgdbType.PLATFORM, (PlatformQuery.Endpoint, PlatformQuery.Fields) },
        { IgdbType.GENRE, (GenreQuery.Endpoint, GenreQuery.Fields) },
        { IgdbType.THEME, (ThemeQuery.Endpoint, ThemeQuery.Fields) },
        {
            IgdbType.PLAYER_PERSPECTIVE,
            (PlayerPerspectiveQuery.Endpoint, PlayerPerspectiveQuery.Fields)
        },
        { IgdbType.GAME_ENGINE, (GameEngineQuery.Endpoint, GameEngineQuery.Fields) },
        //{ IgdbType.COMPANY, (CompanyQuery.Endpoint, CompanyQuery.Fields) },
        { IgdbType.GAME, (GameQuery.Endpoint, GameQuery.Fields) },
    };
}
