using FastEndpoints;
using Ludus.Server.Features.Auth.Extensions;
using Ludus.Server.Features.Common.Games.Services;
using Ludus.Server.Features.DataAccess;
using Marten;

namespace Me.Wishlists.GetAll;

public class Endpoint : EndpointWithoutRequest<GetWishlistedGamesResponse>
{
    public IGameService GameService { get; set; }
    public LudusContext DBContext { get; set; }

    public override void Configure()
    {
        Get("/all");
        Group<MeWishlistGroup>();
    }

    public override async Task HandleAsync(CancellationToken ct)
    {
        var userId = User.GetUserId();
        var wishlistedGamesIds = await DBContext
            .Wishlists.Where(x => x.UserId == userId)
            .Select(w => w.GameId)
            .ToListAsync();

        var previews = await GameService.CreateGameDtoAsync(User, wishlistedGamesIds);

        var response = new GetWishlistedGamesResponse() { Games = previews.ToList() };
        await SendAsync(response);
    }
}
