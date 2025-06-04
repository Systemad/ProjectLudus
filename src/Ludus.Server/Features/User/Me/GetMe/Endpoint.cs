using FastEndpoints;
using Ludus.Server.Features.Common;
using Ludus.Server.Features.User.Common.Models;
using Marten;

namespace Ludus.Server.Features.User.Me.GetMe;

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
            var userId = Guid.Parse(User.Identity.Name);
            await using var session = UserStore.QuerySession();
            var ludusUser = await session.LoadAsync<Common.Models.User>(userId);
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
