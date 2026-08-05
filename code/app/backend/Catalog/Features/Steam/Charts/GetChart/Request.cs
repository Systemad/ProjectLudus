using Catalog.Features.Games.Common.Pagination;

namespace Catalog.Features.Steam.Charts.GetChart;

public sealed class Request : PageRequest
{
    public const string TypeMostPlayed = "most-played";
    public const string TypePopularReleases = "popular-releases";
    public const string TypeHotReleases = "hot-releases";

    public string? Type { get; init; }
}
