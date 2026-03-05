using CatalogAPI.Models;

namespace CatalogAPI;

public record PaginatedResponse<T>(PageMetadata PageInfo, List<PagedItem<T>> Data);

public class PageMetadata
{
    public required string? NextPageCursor { get; init; }
    public required bool HasNextPage { get; init; }
    public required bool HasPreviousPage { get; init; }
    public required long TotalCount { get; init; }
}

public class PagedItem<T>
{
    public required string Cursor { get; init; }
    public required T Item { get; init; }
}

public class GameItem
{
    public long Id { get; set; }
    public string? Name { get; set; }
    public string? Summary { get; set; }
    public string? Storyline { get; set; }
    public long? FirstReleaseDate { get; set; }
    public long? GameType { get; set; }
    public string? CoverUrl { get; set; }
    public int? ReleaseYear { get; set; }
    public float Score { get; set; }
    public List<string>? Themes { get; set; }
    public List<string>? Genres { get; set; }
    public List<string>? Modes { get; set; }
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
            CoverUrl = game.CoverUrl,
            ReleaseYear = game.ReleaseYear,
            Score = game.Score,
            Themes = game.Themes,
            Genres = game.Genres,
            Modes = game.Modes
        };
        return data;
    }
}