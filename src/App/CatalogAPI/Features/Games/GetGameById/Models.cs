using CatalogAPI.Data.Features.Games;

namespace Features.Games.GetGameById;

public class GetGameByIdRequest
{
    public long GameId { get; set; }
}

public record GetGamesByIdResponse(GameDto Game);
