using Catalog.Features.Games.Common.Dtos;

namespace Catalog.Features.Games.Page.GetReleaseData;

public sealed record GamePageReleaseDataDto
{
    public required string Id { get; init; }

    public required string Name { get; init; }

    public required string Slug { get; init; }

    public required IReadOnlyList<GameReleaseDto> Releases { get; init; }
}

public sealed record GameReleaseDto
{
    public string? PlatformName { get; init; }
    public string? PlatformSlug { get; init; }
    public long? ReleaseDate { get; init; }
    public string? Region { get; init; }
    public string? Human { get; init; }
    public ReleaseDateStatusDto? Status { get; init; }
    public PlatformDto? Platform { get; init; }

    public required IReadOnlyList<InvolvedCompanyDto> InvolvedCompanies { get; init; }
}

public sealed record ReleaseDateStatusDto(string Id, string Name);

public sealed record GetGamePageReleaseDataResponse(GamePageReleaseDataDto Data);
