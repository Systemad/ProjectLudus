using FastEndpoints;
using Ludus.Server.Features.Common;
using Ludus.Server.Features.Public.Games.Common.Services;
using Ludus.Shared.Features.Games;
using Marten.Pagination;

namespace Ludus.Server.Features.Public.Games.GetTopRatedGames;

public class Endpoint : Endpoint<GetTopRatedGamesRequest, GetTopRatedGamesResponse>
{
    public IGameStore Store { get; set; }
    public IGameService GameService { get; set; }

    public override void Configure()
    {
        Get("/top");
        AllowAnonymous();
        Group<GamesGroupEndpoint>();
    }

    public override async Task HandleAsync(GetTopRatedGamesRequest req, CancellationToken ct)
    {
        await using var session = Store.QuerySession();

        var games = await session
            .Query<Game>()
            .Where(x => x.GameType.Id == 0)
            .OrderByDescending(x => x.RatingCount)
            .ThenByDescending(x => x.Rating)
            .ToPagedListAsync(req.PageNumber, req.PageSize);
        var previews = await GameService.CreateGameDtoAsync(User, games);
        await SendAsync(
            new GetTopRatedGamesResponse(
                previews,
                games.TotalItemCount,
                games.PageCount,
                games.PageNumber,
                games.IsLastPage
            )
        );
    }
}
