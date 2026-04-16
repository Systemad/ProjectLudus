using System.ComponentModel.DataAnnotations;

namespace CatalogAPI.Features.Games.GetGamePageReleaseData;

public class GamePageReleaseDataDto
{
    [Required] public long GameId { get; set; }
    [Required] public required string GameName { get; set; }
    [Required]
    public required List<GameReleaseDto> Releases { get; set; }
}

public class GameReleaseDto
{
    public string? PlatformName { get; set; }
    public string? PlatformSlug { get; set; }
    public long? ReleaseDate { get; set; }
    public string? Region { get; set; }
    public string? Human { get; set; }
    [Required]
    public required List<CompanyInfoDto> Developers { get; set; }
    [Required]
    public required List<CompanyInfoDto> Publishers { get; set; }
}

public record CompanyInfoDto([Required] long Id, [Required] string Name, string? Slug);
