using CatalogAPI.Features.Games.Common.Dtos;

namespace CatalogAPI.Features.IGDB.GetPopscore;

public sealed record GetPopscoreQuery(long PopularityTypeId, int Limit = 20);

public sealed record GetPopscoreResponse(List<GameBrowseDto> Games);
