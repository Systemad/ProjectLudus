namespace Catalog.Features.Steam.Charts.GetConcurrentUsersChart;

public sealed record ChartPointDto(DateTime Timestamp, int Players);

public sealed record ConcurrentUsersChartResponse(
    string Range,
    string BucketSize,
    List<ChartPointDto> Points
);
