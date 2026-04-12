namespace CatalogAPI.Features.Games.GetGamePageReleaseData;

public class GamePageReleaseDataDto
{
    public long GameId { get; init; }
    public string? GameName { get; init; }
    public List<GameReleaseDto>? Releases { get; init; }
}

public class GameReleaseDto
{
    public string? PlatformName { get; init; }
    public string? PlatformSlug { get; init; }
    public long? ReleaseDate { get; init; }
    public string? Region { get; init; }
    public string? Human { get; init; }
    public List<CompanyInfoDto>? Developers { get; init; }
    public List<CompanyInfoDto>? Publishers { get; init; }
}

public record CompanyInfoDto(long Id, string? Name, string? Slug);
