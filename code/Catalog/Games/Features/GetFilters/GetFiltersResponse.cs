namespace Catalog.Games.Features.GetFilters;

public record GetFiltersResponse(
    IEnumerable<FilterItem> Genres,
    IEnumerable<FilterItem> Platforms,
    IEnumerable<FilterItem> GameTypes,
    IEnumerable<FilterItem> Themes,
    IEnumerable<FilterItem> GameModes,
    IEnumerable<FilterItem> GameEngines,
    IEnumerable<FilterItem> PlayerPerspectives
);
