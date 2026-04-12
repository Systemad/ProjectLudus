using CatalogAPI.Data;

namespace CatalogAPI.Dtos;

public class GameMedia
{
    public List<string>? Screenshots { get; init; }
    public List<GameMediaVideo>? Videos { get; init; }
}

public record GameMediaVideo(string? Name, string? VideoId);
