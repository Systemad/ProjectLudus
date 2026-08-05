namespace Catalog.Features.IGDB.GetStatistics;

public sealed record StatisticsResponse(
    long TotalGames,
    long TotalCompanies,
    long TotalPlatforms,
    long TotalEvents
);
