namespace CatalogAPI.Features.Games.Browse.GetByReleaseDateRange;

public sealed record GetByReleaseDateRangeResponse(
    DateOnly Start, DateOnly End, int Limit, List<GameBrowseDto> Games);
