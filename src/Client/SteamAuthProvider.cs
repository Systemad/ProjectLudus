using System.Security.Claims;
using Microsoft.AspNetCore.Components.Authorization;
using Shared;

namespace Client;

public class SteamAuthProvider : AuthenticationStateProvider
{
    private readonly AuthenticatedUserService _userService;

    public SteamAuthProvider(AuthenticatedUserService userService)
    {
        _userService = userService;
    }

    private static IEnumerable<Claim> GetClaims(LudusUser userInfo)
    {
        return new[]
        {
            new Claim(ClaimTypes.Name, userInfo.Name),
            new Claim(ClaimTypes.NameIdentifier, userInfo.Id.ToString()),
            new Claim("SteamId", userInfo.SteamId),
            new Claim(ClaimTypes.Role, userInfo.Role)
        };
    }

    public override Task<AuthenticationState> GetAuthenticationStateAsync()
    {
        ClaimsIdentity identity;
        if (_userService.IsAuthenticated)
        {
            IEnumerable<Claim> claims = GetClaims(_userService.UserInfo);
            identity = new(claims, "Steam");
        }
        else
        {
            identity = new ClaimsIdentity();
        }

        ClaimsPrincipal user = new(identity);
        AuthenticationState authenticationState = new(user);
        return Task.FromResult(authenticationState);
    }
}