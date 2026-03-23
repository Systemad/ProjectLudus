namespace CatalogAPI.Features.Search;

public record SearchAggregateResult(
    string? Genres,
    string? Themes,
    string? GameModes,
    string? MultiPlayerModes,
    string? PlayerPerspective,
    long Total
);
