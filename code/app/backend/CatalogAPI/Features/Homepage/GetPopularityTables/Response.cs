using CatalogAPI.Features.Games.Common.Dtos;

namespace CatalogAPI.Features.Homepage.GetPopularityTables;

public sealed record PopularityTablesResponse(
    List<GameBrowseDto> MostWishlisted, List<GameBrowseDto> GlobalTopSellers);
