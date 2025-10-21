using CatalogAPI.Data;
using CatalogAPI.Data.Features.Games;
using FastEndpoints;

namespace Features.Games.GetGameById;

public class Endpoint : Endpoint<GetGameByIdRequest, GetGamesByIdResponse>
{
    public SyncDbContext DbContext { get; set; }

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