using System.Security.Claims;
using Ludus.Data;
using Ludus.Shared;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.EntityFrameworkCore;
using Steam.Models.SteamCommunity;
using SteamWebAPI2.Interfaces;
using SteamWebAPI2.Utilities;

namespace Ludus.Server;

public class ValidationHelper
{
    private const int SteamIdStartIndex = 37;

    public static async Task SignIn(CookieSignedInContext context)
    {
        if (context == null)
        {
            throw new ArgumentNullException(nameof(context));
        }

        using var db = new AppDbContext();

        var steamId = context.Principal.FindFirst(ClaimTypes.NameIdentifier).Value[SteamIdStartIndex..];
        var user = await db.Users.FirstOrDefaultAsync(x =>
            x.SteamId == steamId);

        if (user != null)
        {
            return;
        }

        var httpClientFactory =
            context.HttpContext.RequestServices.GetRequiredService<IHttpClientFactory>();
        var logger =
            context.HttpContext.RequestServices.GetRequiredService<ILogger<ValidationHelper>>();
        var steamFactory =
            context.HttpContext.RequestServices.GetRequiredService<SteamWebInterfaceFactory>();
        var httpClient = httpClientFactory.CreateClient();
        httpClient.Timeout = TimeSpan.FromSeconds(3);

        PlayerSummaryModel playerSummary = null;
        try
        {
            ISteamWebResponse<PlayerSummaryModel> steamWebResponse = await steamFactory
                .CreateSteamWebInterface<SteamUser>(httpClient).GetPlayerSummaryAsync(ulong.Parse(steamId));
            playerSummary = steamWebResponse.Data;
        }
        catch (Exception e)
        {
            logger.LogError(e, "An exception occurated when downloading player summaries");
        }

        user = new LudusUser()
        {
            SteamId = steamId,
            Role = RoleConstants.DefaultRoleId
        };

        if (playerSummary != null)
        {
            user.Name = playerSummary.Nickname;

            try
            {
                var avatarContentBytes = await httpClient.GetByteArrayAsync(playerSummary.AvatarFullUrl);
                var img = new LudusUserImage()
                {
                    Name = "steam_avatar.jpg",
                    Content = avatarContentBytes,
                    ContentType = "image/jpeg"
                };
                user.LudusUserImage = img;
            }
            catch (Exception e)
            {
                logger.LogError(e, $"An exception occurated when downloading player avatar {playerSummary.SteamId}");
            }
        }

        db.Users.Add(user);
        await db.SaveChangesAsync();
    }

    public static async Task Validate(CookieValidatePrincipalContext context)
    {
        var steamId = context.Principal.FindFirst(ClaimTypes.NameIdentifier).Value[SteamIdStartIndex..];

        using var db = new AppDbContext();
        var user = await db.Users.FirstOrDefaultAsync(x => x.SteamId == steamId);
        var claims = new List<Claim>
        {
            new Claim(ClaimTypes.NameIdentifier, steamId),
            new Claim(ClaimTypes.Name, user.Id.ToString()),
            new Claim(ClaimTypes.Role, user.Role)
        };

        context.ReplacePrincipal(new ClaimsPrincipal(new ClaimsIdentity(claims, "Steam")));
    }
}