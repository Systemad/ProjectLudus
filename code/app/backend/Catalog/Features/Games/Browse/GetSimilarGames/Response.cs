using Catalog.Features.Games.Common.Dtos;

namespace Catalog.Features.Games.Browse.GetSimilarGames;

public sealed record GetSimilarGamesResponse(List<GameBrowseDto> Games);
