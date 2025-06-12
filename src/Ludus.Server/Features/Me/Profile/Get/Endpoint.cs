using FastEndpoints;
using Ludus.Server.Features.Auth.Extensions;
using Ludus.Server.Features.Common.Users.Models;
using Ludus.Server.Features.DataAccess;
using Microsoft.EntityFrameworkCore;

namespace Me.Profile.Get;

public class Endpoint : EndpointWithoutRequest<GetMeResponse>
{
    public LudusContext DbContext { get; set; }

    public override void Configure()
    {
        Get("/me");
        Group<MeProfileGroup>();
        //AllowAnonymous();
    }

    public override async Task HandleAsync(CancellationToken ct)
    {
        if (User.Identity?.IsAuthenticated ?? false)
        {
            var userId = User.GetUserId();
            var ludusUser = await DbContext.Users.FirstOrDefaultAsync(x => x.Id == userId);
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
                CreatedDate = ludusUser.CreatedDate,
                UserImage = ludusUser.UserImage,
            };
            await SendOkAsync(new GetMeResponse(userDto));
        }

        await SendUnauthorizedAsync();
    }
}
