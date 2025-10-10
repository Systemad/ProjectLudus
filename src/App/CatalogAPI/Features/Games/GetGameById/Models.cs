
namespace Features.Games.GetGameById;

public class GetGameByIdRequest
{
    public long GameId { get; set; }
}

public record GetGamesByIdResponse(IgdbGameDto Game);
