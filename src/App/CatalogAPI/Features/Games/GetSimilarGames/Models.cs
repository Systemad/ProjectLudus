using WebAPI.Features.Common.Games.Models;

namespace Features.Games.GetSimilarGames;

public class GetSimilarGamesRequest
{
    public long GameId { get; set; }
}

public class GetSimilarGamesResponse
{
    public List<GameDto> SimilarGames { get; set; }
}
