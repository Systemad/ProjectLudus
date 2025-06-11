using FastEndpoints;
using Ludus.Server.Features.Auth.Extensions;
using Ludus.Server.Features.Common.Games.Services;
using Ludus.Server.Features.DataAccess;
using Marten;
using Me.Hypes;
using Me.Hypes.GetAll;

namespace Me.Hyped.GetAll;

public class Endpoint : EndpointWithoutRequest<GetHypesGamesResponse>
{
    public IGameService GameService { get; set; }
    public LudusContext DBContext { get; set; }

    public override void Configure()
    {
        Get("/all");
        Group<MeHypesGroup>();
    }

    public override async Task HandleAsync(CancellationToken ct)
    {
        var userId = User.GetUserId();
        var hypedGamesIds = await DBContext
            .GameHypes.Where(x => x.UserId == userId)
            .Select(w => w.GameId)
            .ToListAsync();

        var previews = await GameService.CreateGameDtoAsync(User, hypedGamesIds);

        var response = new GetHypesGamesResponse() { Games = previews.ToList() };
        await SendAsync(response);
    }
}
