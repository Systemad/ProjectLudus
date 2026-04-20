namespace CatalogAPI.Features.Games.GetByReleaseDateRange;

public record GetByReleaseDateRangeQuery(DateOnly Start, DateOnly End, int Limit);
