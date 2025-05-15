using System.Security.Claims;
using Ludus.Server.Features.User.Models;
using Ludus.Shared;
using Marten;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using Steam.Models.SteamCommunity;
using SteamWebAPI2.Interfaces;
using SteamWebAPI2.Utilities;

namespace Ludus.Server.Features.Auth;

public static class AuthEndpoints
{
    private const int SteamIdStartIndex = 37;

    public static IEndpointRouteBuilder MapAuthEndpoints(this IEndpointRouteBuilder routes)
    {
        routes.MapGet("~/signout", SignOutAsync);
        routes.MapGet("~/signin", SignIn);
        routes.MapGet("/Auth/steamresponse", SteamResponse);

        return routes;
    }

    private static IResult SignIn(HttpContext httpContext)
    {
        Console.WriteLine("Siginin");
        return Results.Challenge(
            new AuthenticationProperties
            {
                RedirectUri = "/Auth/steamresponse",
                IsPersistent = false,
                IssuedUtc = DateTime.Now,
                ExpiresUtc = DateTimeOffset.UtcNow.AddDays(7),
            },
            ["Steam"]
        );
    }

    private static async Task<IResult> SteamResponse(IDocumentStore db, HttpContext context)
    {
        var authenticationResult = await context.AuthenticateAsync(
            CookieAuthenticationDefaults.AuthenticationScheme
        );
        if (!authenticationResult.Succeeded)
            return Results.Redirect("/");

        var steamId = authenticationResult.Principal.FindFirst(ClaimTypes.NameIdentifier).Value[
            SteamIdStartIndex..
        ];

        //var claims = new List<Claim>(authenticationResult.Principal.Claims);
        //var steamIdClaim = claims.FirstOrDefault(x => x.Type == ClaimTypes.NameIdentifier)?.Value;
        await using var session = db.LightweightSession();

        var user = await session
            .Query<User.Models.User>()
            .FirstOrDefaultAsync(x => x.SteamId == steamId);
        if (user is not null)
        {
            var httpClientFactory =
                context.RequestServices.GetRequiredService<IHttpClientFactory>();
            var steamFactory =
                context.RequestServices.GetRequiredService<SteamWebInterfaceFactory>();
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
                //logger.LogWarning("Could not fetch Steam profile, user not created.");
                return TypedResults.Redirect("/");
            }

            user = new User.Models.User
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
            .Query<User.Models.User>()
            .FirstOrDefaultAsync(x => x.SteamId == steamId);

        var returnClaims = new List<Claim>
        {
            new Claim(ClaimTypes.NameIdentifier, newUser.SteamId),
            new Claim(ClaimTypes.Name, newUser.Id.ToString()),
            new Claim(ClaimTypes.Role, newUser.Role),
        };
        var claimsIdentity = new ClaimsIdentity(
            returnClaims,
            CookieAuthenticationDefaults.AuthenticationScheme
        );
        var authProperties = new AuthenticationProperties();

        await context.SignInAsync(
            CookieAuthenticationDefaults.AuthenticationScheme,
            new ClaimsPrincipal(claimsIdentity),
            authProperties
        );
        return Results.Redirect("/");
    }

    private static async Task<IResult> SignOutAsync(
        HttpContext httpContext,
        [FromQuery] string returnUrl = "/"
    )
    {
        Console.WriteLine("SignOutAsync");

        await httpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);

        return Results.Redirect(returnUrl);
    }
}
