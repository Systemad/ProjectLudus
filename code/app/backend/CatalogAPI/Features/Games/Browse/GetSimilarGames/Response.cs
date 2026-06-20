using CatalogAPI.Features.Games.Common.Dtos;

namespace CatalogAPI.Features.Games.Browse.GetSimilarGames;

public sealed record GetSimilarGamesResponse(List<GameBrowseDto> Games);
