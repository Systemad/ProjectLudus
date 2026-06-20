using CatalogAPI.Features.Games.Common.Dtos;

namespace CatalogAPI.Features.IGDB.GetMostAnticipated;

/// <summary>Most anticipated upcoming games.</summary>
public sealed record AnticipatedGamesResponse(List<GameBrowseDto> Games);
