using System.Text.Json.Serialization;

namespace CatalogAPI.Models;

public sealed class GameSearchRequest
{
    public string? Name { get; init; }
    public string[]? Genres { get; init; } = [];
    public string[]? Themes { get; init; } = [];
    public string[]? Modes { get; init; } = [];
    public string? AfterCursor { get; set; }
    public int? PageSize { get; set; } = 40;
}

//[JsonConverter(typeof(JsonStringEnumConverter))]
public enum SortOption
{
    Relevancy,
    //ReleaseDate,
    //Rating
}

//public SortOption? SortOption { get; set; } = Models.SortOption.Relevancy;
    
// public long? Cursor { get; init; }