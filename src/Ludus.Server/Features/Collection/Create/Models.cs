using Ludus.Server.Features.Collection.Common;

namespace Ludus.Server.Features.Collection.Create;

public record TrackGameStateResponse(UserGameStateDto GameState);

public class TrackGameStateRequest : IGameStateRequest
{
    public long GameId { get; set; }
}

/*
public class TrackGameValidator : Validator<TrackCollectionRequest>
{
    public TrackGameValidator()
    {
        RuleFor(x => x.GameId).GreaterThan(0).WithMessage("Game ID cannot be null!");
    }
}
*/
