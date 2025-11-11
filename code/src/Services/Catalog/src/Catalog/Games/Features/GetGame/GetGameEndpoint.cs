using Catalog.Data;
using Catalog.Games.Dtos;
using FastEndpoints;

namespace Catalog.Games.Features.GetGame;

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
