namespace CatalogAPI.Models;

public sealed class GameSearchRequest
{
    public string? Name { get; init; }
    public string[] Genres { get; init; } = [];
    public string[] Themes { get; init; } = [];
    public string[] Modes  { get; init; } = [];
    public string Order    { get; init; } = "relevance";
    public long? Cursor    { get; init; }
    public int PageSize    { get; init; } = 40;
}