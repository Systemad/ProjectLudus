using System.ComponentModel.DataAnnotations;
using CatalogAPI.Features.Games.Common.Dtos;

namespace CatalogAPI.Features.Games.Page.GetReleaseData;

public class GamePageReleaseDataDto
{
    [Required]
    public required long Id { get; set; }

    [Required]
    public required string Name { get; set; }

    [Required]
    public required string Slug { get; set; }

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
    public ReleaseDateStatusDto? Status { get; set; }
    public PlatformDto? Platform { get; set; }

    [Required]
    public required List<InvolvedCompanyDto> InvolvedCompanies { get; set; }
}

public record ReleaseDateStatusDto([Required] long Id, [Required] string Name);

public sealed record GetGamePageReleaseDataResponse(GamePageReleaseDataDto Data);