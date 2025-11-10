using Catalog.Api.Data;
using Catalog.Api.Data.Features.Games;
using Catalog.Games.Features;
using FastEndpoints;

namespace Features.Games.GetGameById;

public class GetGameByIdRequest
{
    public long GameId { get; set; }
}

public record GetGamesByIdResponse(GameDto Game);

public class GetGameEndpoint : Endpoint<GetGameByIdRequest>
{
    public CatalogContext DbContext { get; set; }

    public override void Configure()
    {
        Get("/{GameId}");
        Group<GamesGroupEndpoint>();
        AllowAnonymous();
    }

    public override async Task HandleAsync(GetGameByIdRequest req, CancellationToken ct)
    {
        if (req.GameId == 0)
        {
            AddError(r => r.GameId, "Game IDs cannot be empty");
            ThrowIfAnyErrors();
        }

        await Send.OkAsync(new GetGamesByIdResponse(new GameDto()), ct);
    }
}
