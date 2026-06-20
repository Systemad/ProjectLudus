using CatalogAPI.Features.Games.Common.Dtos;

namespace CatalogAPI.Features.Steam.Charts.GetChart;

public sealed record GamesResponse(List<GameBrowseDto> Games);
