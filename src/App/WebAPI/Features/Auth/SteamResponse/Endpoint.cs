using System.Security.Claims;
using FastEndpoints;
using FastEndpoints.Security;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.EntityFrameworkCore;
using Shared;
using Steam.Models.SteamCommunity;
using SteamWebAPI2.Interfaces;
using SteamWebAPI2.Utilities;
using WebAPI.Features.Common.Users.Models;
using WebAPI.Features.DataAccess;

namespace Auth.SteamResponse;

public class Endpoint : EndpointWithoutRequest<IResult>
{
    private const int SteamIdStartIndex = 37;
    public LudusContext DbContext { get; set; }

    public override void Configure()
    {
        Get("/Auth/steamresponse");
        AuthSchemes(CookieAuthenticationDefaults.AuthenticationScheme);
        AuthSchemes("Steam");
        AllowAnonymous();
    }

    public override async Task HandleAsync(CancellationToken ct)
    {
        var authenticationResult = await HttpContext.AuthenticateAsync(
            CookieAuthenticationDefaults.AuthenticationScheme
        );

        if (!authenticationResult.Succeeded)
            await SendRedirectAsync("/");
        var steamId = authenticationResult.Principal.FindFirst(ClaimTypes.NameIdentifier).Value[
            SteamIdStartIndex..
        ];

        var user = await DbContext
            .Users.AsTracking()
            .FirstOrDefaultAsync(x => x.SteamId == steamId, cancellationToken: ct);

        if (user is null)
        {
            var httpClientFactory =
                HttpContext.RequestServices.GetRequiredService<IHttpClientFactory>();
            var steamFactory =
                HttpContext.RequestServices.GetRequiredService<SteamWebInterfaceFactory>();
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
                // logger.LogError(e, "An exception occured when downloading player summary");
            }

            if (playerSummary == null)
            {
                await SendRedirectAsync("/");
                //logger.LogWarning("Could not fetch Steam profile, user not created.");
                //Response = TypedResults.Redirect("/");
            }

            user = new User
            {
                Name = playerSummary.Nickname,
                SteamId = steamId,
                Role = RoleConstants.DefaultRoleId,
                CreatedDate = DateTime.UtcNow,
            };

            try
            {
                var avatarContentBytes = await httpClient.GetByteArrayAsync(
                    playerSummary.AvatarFullUrl, ct);
                var img = new UserImage()
                {
                    Name = "steam_avatar.jpg",
                    UserId = user.Id,
                    Content = avatarContentBytes,
                    ContentType = "image/jpeg",
                    CreatedDate = DateTime.UtcNow,
                };
                user.UserImage = img;
            }
            catch (Exception e)
            {
                //logger.LogError(
                //    e,
                //    $"An exception occured when fetching player data {playerSummary.SteamId}"
                //);
            }

            DbContext.Users.Add(user);
            await DbContext.SaveChangesAsync(ct);
        }

        var newUser = await DbContext.Users.FirstOrDefaultAsync(x => x.SteamId == steamId, cancellationToken: ct);

        await CookieAuth.SignInAsync(u =>
        {
            u.Claims.Add(new Claim(ClaimTypes.NameIdentifier, newUser.SteamId));
            u.Claims.Add(new Claim(ClaimTypes.Name, newUser.Id.ToString()));
            u.Claims.Add(new Claim(ClaimTypes.Role, newUser.Role));
        });

        await SendRedirectAsync("/");
    }
}
