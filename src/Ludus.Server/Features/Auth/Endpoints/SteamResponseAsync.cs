using System.Security.Claims;
using FastEndpoints;
using FastEndpoints.Security;
using Ludus.Server.Features.User.Common.Models;
using Ludus.Shared;
using Marten;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Steam.Models.SteamCommunity;
using SteamWebAPI2.Interfaces;
using SteamWebAPI2.Utilities;

namespace Ludus.Server.Features.Auth.Endpoints;

public class SteamResponseAsync : EndpointWithoutRequest<IResult>
{
    private const int SteamIdStartIndex = 37;
    public IDocumentStore UserStore { get; set; }

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

        await using var session = UserStore.LightweightSession();

        var user = await session
            .Query<User.Common.Models.User>()
            .FirstOrDefaultAsync(x => x.SteamId == steamId);
        if (user is not null)
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

            user = new User.Common.Models.User
            {
                Name = playerSummary.Nickname,
                SteamId = steamId,
                Role = RoleConstants.DefaultRoleId,
                CreatedDate = DateTime.Today,
            };

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
                    CreatedDate = DateTime.UtcNow,
                };
                user.UserImage = img;
                user.AvatarImageId = img.Id;
            }
            catch (Exception e)
            {
                //logger.LogError(
                //    e,
                //    $"An exception occured when fetching player data {playerSummary.SteamId}"
                //);
            }
            session.Store(user);
            await session.SaveChangesAsync();
        }

        var newUser = await session
            .Query<User.Common.Models.User>()
            .FirstOrDefaultAsync(x => x.SteamId == steamId);

        await CookieAuth.SignInAsync(u =>
        {
            u.Claims.Add(new Claim(ClaimTypes.NameIdentifier, newUser.SteamId));
            u.Claims.Add(new Claim(ClaimTypes.Name, newUser.Id.ToString()));
            u.Claims.Add(new Claim(ClaimTypes.Role, newUser.Role));
        });

        await SendRedirectAsync("/");
    }
}
