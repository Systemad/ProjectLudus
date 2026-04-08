namespace CatalogAPI.Features.Games.GetByReleaseDateRange;

public record GetByReleaseDateRangeQuery(long From, long To, int Limit);
