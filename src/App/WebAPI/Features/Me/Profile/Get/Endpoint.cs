using FastEndpoints;
using Microsoft.EntityFrameworkCore;
using WebAPI.Features.Auth.Extensions;
using WebAPI.Features.Common.Users.DTO;
using WebAPI.Features.DataAccess;

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
            await SendOkAsync(new GetMeResponse(userDto), ct);
            return;
        }

        await SendUnauthorizedAsync(ct);
    }
}
