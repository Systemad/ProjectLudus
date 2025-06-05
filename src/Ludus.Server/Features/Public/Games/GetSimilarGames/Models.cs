using Ludus.Server.Features.Common.Games.Models;
using Ludus.Server.Features.Public.Games.Common;

namespace Ludus.Server.Features.Public.Games.GetSimilarGames;

public class GetSimilarGamesRequest
{
    public long GameId { get; set; }
}

public class GetSimilarGamesResponse(List<GameDto> SimilarGames);
