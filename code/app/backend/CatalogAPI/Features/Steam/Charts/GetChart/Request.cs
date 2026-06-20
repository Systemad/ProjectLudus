using Microsoft.AspNetCore.Mvc;

namespace CatalogAPI.Features.Steam.Charts.GetChart;

public class Request
{
    public const string TypeMostPlayed = "most-played";
    public const string TypePopularReleases = "popular-releases";
    public const string TypeHotReleases = "hot-releases";

    [FromQuery]
    public string? Type { get; init; }

    [FromQuery]
    public int? Limit { get; init; }
}
