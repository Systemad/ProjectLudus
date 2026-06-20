using CatalogAPI.Features.Games.Common.Dtos;

namespace CatalogAPI.Features.Homepage.GetUpcoming;

public sealed record UpcomingResponse(List<GameBrowseDto> Games);
