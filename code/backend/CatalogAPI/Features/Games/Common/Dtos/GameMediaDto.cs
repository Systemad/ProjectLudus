using System.ComponentModel.DataAnnotations;

namespace CatalogAPI.Features.Games.Common.Dtos;

public class GameMediaDto
{
    [Required]
    public required List<string> Screenshots { get; set; } = [];

    [Required]
    public required List<GameMediaVideoDto> Videos { get; set; } = [];
}

public record GameMediaVideoDto([Required] string Name, [Required] string VideoId);
