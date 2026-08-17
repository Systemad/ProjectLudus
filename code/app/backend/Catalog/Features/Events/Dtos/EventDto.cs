using Catalog.Features.Games.Browse.GetMedia;
using Catalog.Features.Games.Common.Dtos;

namespace Catalog.Features.Events.Dtos;

public sealed record EventDto
{
    public required string Id { get; init; }

    public required string Name { get; init; }

    public string? Slug { get; init; }

    public string? Description { get; init; }

    public string? LiveStreamUrl { get; init; }

    public DateTime? StartTimeUtc { get; init; }

    public DateTime? EndTimeUtc { get; init; }

    public string? TimeZone { get; init; }

    public string? LogoImageId { get; init; }

    public required IReadOnlyList<GameBrowseDto> Games { get; init; }

    public required IReadOnlyList<GameMediaVideoDto> Videos { get; init; }
}
