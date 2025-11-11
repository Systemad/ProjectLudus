using FastEndpoints;
using Microsoft.EntityFrameworkCore;
using Ludus.Api.Features.Auth.Extensions;
using Ludus.Api.Features.Common.Users.DTO;
using Ludus.Api.Features.DataAccess;

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
            var ludusUser = await DbContext
                .Users.Include(user => user.UserImage)
                .FirstOrDefaultAsync(x => x.Id == userId, cancellationToken: ct);
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
                UserImage = new UserImageDto
                {
                    Id = ludusUser.UserImage.Id,
                    Name =  ludusUser.UserImage.Name,
                    ContentType =  ludusUser.UserImage.ContentType,
                    Content =  ludusUser.UserImage.Content,
                    CreatedDate =  ludusUser.UserImage.CreatedDate
                },
            };
            await Send.OkAsync(new GetMeResponse(userDto), ct);
            return;
        }

        await Send.UnauthorizedAsync(ct);
    }
}
