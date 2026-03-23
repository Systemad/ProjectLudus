using CatalogAPI.Models;

namespace CatalogAPI;

public record SearchResponse<T>(
    long TotalCount,
    int PageSize,
    List<PagedItem<T>> Data,
    List<FacetGroupResponse> AggregationBuckets,
    PageMetadata PageMetadata
);

public class PageMetadata
{
    public required string? NextPageCursor { get; init; }
    public required bool HasNextPage { get; init; }
    public required bool HasPreviousPage { get; init; }
}

public class PagedItem<T>
{
    public required string Cursor { get; init; }
    public required T Item { get; init; }
}

public static class ResponseMapper
{
    public static GameItem MapTo(this GameSearchFacet game)
    {
        var data = new GameItem
        {
            Id = game.Id,
            Name = game.Name,
            Summary = game.Summary,
            Storyline = game.Summary,
            FirstReleaseDate = game.FirstReleaseDate,
            GameType = game.GameType,
            GameStatus = game.GameStatus,
            CoverUrl = game.CoverUrl,
            ReleaseYear = game.ReleaseYear,
            Score = game.Score,
            Themes = game.Themes,
            Genres = game.Genres,
            GameModes = game.GameModes,
            Platforms = game.Platforms,
            GameEngines = game.GameEngines,
            PlayerPerspectives = game.PlayerPerspectives,
            Publishers = game.Publishers,
            Developers = game.Developers,
            MultiplayerModes = game.MultiplayerModes,
        };
        return data;
    }
}
