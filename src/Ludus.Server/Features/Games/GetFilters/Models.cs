namespace Ludus.Server.Features.Games.GetFilters;

public record GenreFilter(long Id, string Name);

public record PlatformFilter(long Id, string Name);

public record GameTypeFilter(long Id, string Name);

public record ThemeFilter(long Id, string Name);

public record GameModeFilter(long Id, string Name);

public record GameEngineFilter(long Id, string Name);

public record PlayerPerspectiveFilter(long Id, string Name);

public record GetFiltersResponse(
    IEnumerable<GenreFilter> Genres,
    IEnumerable<PlatformFilter> Platforms,
    IEnumerable<GameTypeFilter> GameTypes,
    IEnumerable<ThemeFilter> Themes,
    IEnumerable<GameModeFilter> GameModes,
    IEnumerable<GameEngineFilter> GameEngines,
    IEnumerable<PlayerPerspectiveFilter> PlayerPerspectives
);
