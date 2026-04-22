using System.ComponentModel.DataAnnotations;
using CatalogAPI.Features.Games.Common.Dtos;
using CatalogAPI.Features.Games.GetMedia;

namespace CatalogAPI.Features.Events.Dtos;

public class EventDto
{
    [Required]
    public required long Id { get; init; }

    [Required]
    public required string Name { get; init; }

    public string? Slug { get; init; }

    public string? Description { get; init; }

    public string? LiveStreamUrl { get; init; }

    public DateTime? StartTimeUtc { get; init; }

    public DateTime? EndTimeUtc { get; init; }

    public string? TimeZone { get; init; }

    public string? LogoImageId { get; init; }

    [Required]
    public List<GameDto> Games { get; set; } = [];

    [Required]
    public List<GameMediaVideoDto> Videos { get; set; } = [];
}
