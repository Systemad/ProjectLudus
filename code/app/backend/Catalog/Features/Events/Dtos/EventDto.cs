using System.ComponentModel.DataAnnotations;
using Catalog.Features.Games.Browse.GetMedia;
using Catalog.Features.Games.Common.Dtos;

namespace Catalog.Features.Events.Dtos;

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
    public List<GameBrowseDto> Games { get; set; } = [];

    [Required]
    public List<GameMediaVideoDto> Videos { get; set; } = [];
}
