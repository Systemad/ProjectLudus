using Shared.Features;
using SyncService.Features.Companies;
using SyncService.Features.GameEngines;
using SyncService.Features.Games;
using SyncService.Features.Platforms;

namespace SyncService.Features;

public static class Endpoints
{
    public static Dictionary<IgdbReference, (string Endpoint, List<string> Fields)> QueryMaps =
        new()
        {
            { IgdbReference.PLATFORM, (PlatformQuery.Endpoint, PlatformQuery.Fields) },
            { IgdbReference.GAME_ENGINE, (GameEngineQuery.Endpoint, GameEngineQuery.Fields) },
            { IgdbReference.COMPANY, ("", [""]) },
            { IgdbReference.THEME, ("", [""]) },
            { IgdbReference.GENRE, ("", [""]) },
            { IgdbReference.GAME, (GameQuery.Endpoint, GameQuery.Fields) },
        };
}
