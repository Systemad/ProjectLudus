using Ludus.Shared;
using Marten;
using Marten.Schema;

namespace Ludus.Server.Features.User.Seed;

public class InitialData : IInitialData
{
    public async Task Populate(IDocumentStore store, CancellationToken cancellation)
    {
        await using var session = store.LightweightSession();
        var user = new Models.User
        {
            Name = "Dan",
            Role = RoleConstants.AdminRoleId,
            SteamId = "defaultid",
            CreateDate = DateTime.Now,
        };

        session.Store(user);
        await session.SaveChangesAsync();
    }
}

/*
 *         user = new User.User { SteamId = steamId, Role = RoleConstants.DefaultRoleId };

        if (playerSummary != null)
        {
            user.Name = playerSummary.Nickname;

            try
            {
                var avatarContentBytes = await httpClient.GetByteArrayAsync(
                    playerSummary.AvatarFullUrl
                );
                var img = new UserImage()
                {
                    Name = "steam_avatar.jpg",
                    UserId = user.Id,
                    Content = avatarContentBytes,
                    ContentType = "image/jpeg",
                };
                user.UserImage = img;
            }
 */
