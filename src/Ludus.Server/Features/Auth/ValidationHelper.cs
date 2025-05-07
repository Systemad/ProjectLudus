using System.Security.Claims;
using Ludus.Server.Features.User.Models;
using Ludus.Shared;
using Marten;
using Microsoft.AspNetCore.Authentication.Cookies;
using Steam.Models.SteamCommunity;
using SteamWebAPI2.Interfaces;
using SteamWebAPI2.Utilities;

namespace Ludus.Server.Features.Auth;

public class ValidationHelper
{
    private const int SteamIdStartIndex = 37;

    public static async Task SignIn(CookieSignedInContext context)
    {
        if (context == null)
        {
            throw new ArgumentNullException(nameof(context));
        }

        var services = context.HttpContext.RequestServices;
        var userStore = services.GetRequiredService<IDocumentStore>();
        await using var db = userStore.LightweightSession();

        var steamId = context.Principal.FindFirst(ClaimTypes.NameIdentifier).Value[
            SteamIdStartIndex..
        ];
        var user = await db.Query<User.Models.User>()
            .FirstOrDefaultAsync(x => x.SteamId == steamId);

        if (user != null)
        {
            return;
        }

        var httpClientFactory =
            context.HttpContext.RequestServices.GetRequiredService<IHttpClientFactory>();
        var logger = context.HttpContext.RequestServices.GetRequiredService<
            ILogger<ValidationHelper>
        >();
        var steamFactory =
            context.HttpContext.RequestServices.GetRequiredService<SteamWebInterfaceFactory>();
        var httpClient = httpClientFactory.CreateClient();
        httpClient.Timeout = TimeSpan.FromSeconds(3);

        PlayerSummaryModel playerSummary = null;
        try
        {
            ISteamWebResponse<PlayerSummaryModel> steamWebResponse = await steamFactory
                .CreateSteamWebInterface<SteamUser>(httpClient)
                .GetPlayerSummaryAsync(ulong.Parse(steamId));
            playerSummary = steamWebResponse.Data;
        }
        catch (Exception e)
        {
            logger.LogError(e, "An exception occured when downloading player summary");
        }

        user = new User.Models.User { SteamId = steamId, Role = RoleConstants.DefaultRoleId };

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
                user.AvatarImageId = img.Id;
            }
            catch (Exception e)
            {
                logger.LogError(
                    e,
                    $"An exception occured when fetching player data {playerSummary.SteamId}"
                );
            }
        }

        db.Store(user);
        await db.SaveChangesAsync();
    }

    public static async Task Validate(CookieValidatePrincipalContext context)
    {
        var steamId = context.Principal.FindFirst(ClaimTypes.NameIdentifier).Value[
            SteamIdStartIndex..
        ];

        var services = context.HttpContext.RequestServices;
        var userStore = services.GetRequiredService<IDocumentStore>();
        await using var db = userStore.LightweightSession();

        var user = await db.Query<User.Models.User>()
            .FirstOrDefaultAsync(x => x.SteamId == steamId);
        var claims = new List<Claim>
        {
            new Claim(ClaimTypes.NameIdentifier, steamId),
            new Claim(ClaimTypes.Name, user.Id.ToString()),
            new Claim(ClaimTypes.Role, user.Role),
        };

        context.ReplacePrincipal(new ClaimsPrincipal(new ClaimsIdentity(claims, "Steam")));
    }
}
