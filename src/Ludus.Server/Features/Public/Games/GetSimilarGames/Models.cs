using Ludus.Server.Features.Common.Games.Models;

namespace Public.Games.GetSimilarGames;

public class GetSimilarGamesRequest
{
    public long GameId { get; set; }
}

public class GetSimilarGamesResponse
{
    public List<GameDto> SimilarGames { get; set; }
}
