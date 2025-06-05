using FastEndpoints;
using Ludus.Server.Features.Auth.Extensions;
using Ludus.Server.Features.Common;
using Ludus.Server.Features.Common.Users.Models;
using Marten;

namespace Ludus.Server.Features.Me.Profile.Get;

public class Endpoint : EndpointWithoutRequest<GetMeResponse>
{
    public IDocumentStore UserStore { get; set; }

    public override void Configure()
    {
        Get($"/{ApiRoutes.Users}/me");
        //AllowAnonymous();
    }

    public override async Task HandleAsync(CancellationToken ct)
    {
        if (User.Identity?.IsAuthenticated ?? false)
        {
            var userId = User.GetUserId();
            await using var session = UserStore.QuerySession();
            var ludusUser = await session.LoadAsync<Features.Common.Users.Models.User>(userId);
            if (ludusUser is null)
            {
                ThrowError("User doesn't exist!");
            }
            var userDto = new UserDto
            {
                Id = ludusUser.Id,
                Name = ludusUser.Name,
                Role = ludusUser.Name,
                SteamId = ludusUser.SteamId,
                AvatarImageId = ludusUser.AvatarImageId,
                CreatedDate = ludusUser.CreatedDate,
                UserImage = ludusUser.UserImage,
            };
            await SendOkAsync(new GetMeResponse(userDto));
        }

        await SendUnauthorizedAsync();
    }
}
