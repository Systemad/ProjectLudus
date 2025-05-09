namespace Ludus.Server.Features.Games.Filters;

public record GenreFilter(long Id, string Name);

public record PlatformFilter(long Id, string Name);

public record GameTypeFilter(long Id, string Name);

public record ThemeFilter(long Id, string Name);

public record GameModeFilter(long Id, string Name);

public record GameEngineFilter(long Id, string Name);
