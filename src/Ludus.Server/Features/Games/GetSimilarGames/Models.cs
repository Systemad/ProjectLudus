using Ludus.Server.Features.Games.Common;

namespace Ludus.Server.Features.Games.GetSimilarGames;

public class GetSimilarGamesRequest
{
    public long GameId { get; set; }
}

public class GetSimilarGamesResponse(List<GameDto> SimilarGames);
