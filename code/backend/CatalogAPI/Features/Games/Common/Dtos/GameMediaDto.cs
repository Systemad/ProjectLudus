namespace CatalogAPI.Features.Games.Common.Dtos;

public class GameMediaDto
{
    public List<string>? Screenshots { get; init; }
    public List<GameMediaVideoDto>? Videos { get; init; }
}

public record GameMediaVideoDto(string? Name, string? VideoId);
