using Ludus.Shared;

namespace Ludus.Server.Features.User.Seed;

public static class InitialDatasets
{
    public static readonly Models.User[] Users =
    [
        new Models.User
        {
            //Id = Guid.NewGuid(),
            Name = "Dan",
            Role = RoleConstants.AdminRoleId,
            SteamId = "010101",
            //AvatarImageId = null,
            CreateDate = DateTime.Now,
            //UserImage = null,
            //GameStatuses = null,
            //GameLists = null,
        },
    ];
}
