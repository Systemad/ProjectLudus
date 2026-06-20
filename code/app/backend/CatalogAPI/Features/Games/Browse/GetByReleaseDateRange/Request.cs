namespace CatalogAPI.Features.Games.Browse.GetByReleaseDateRange;

public record Request(DateOnly Start, DateOnly End, int Limit);
