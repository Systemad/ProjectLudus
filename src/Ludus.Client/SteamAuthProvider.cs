using System.Security.Claims;
using Ludus.Client.Models;
using Microsoft.AspNetCore.Components.Authorization;

namespace Ludus.Client;

public class SteamAuthProvider : AuthenticationStateProvider
{
    private readonly AuthenticatedUserService _userService;
    private readonly HttpClient _httpClient;
    private bool authenticated = false;

    private readonly ClaimsPrincipal unauthenticated = new(new ClaimsIdentity());

    public SteamAuthProvider(AuthenticatedUserService userService, HttpClient httpClient)
    {
        _userService = userService;
        _httpClient = httpClient;
    }

    private static IEnumerable<Claim> GetClaims(UserDto userInfo)
    {
        return new[]
        {
            new Claim(ClaimTypes.Name, userInfo.Name),
            new Claim(ClaimTypes.NameIdentifier, userInfo.Id.ToString()),
            new Claim("SteamId", userInfo.SteamId),
            new Claim(ClaimTypes.Role, userInfo.Role),
        };
    }

    public override Task<AuthenticationState> GetAuthenticationStateAsync()
    {
        //ClaimsIdentity identity;
        try
        {
            if (_userService.IsAuthenticated && _userService.User is not null)
            {
                IEnumerable<Claim> claims = GetClaims(_userService.User);
                var identity = new ClaimsIdentity(claims, "Steam");
                return Task.FromResult(new AuthenticationState(new ClaimsPrincipal(identity)));
            }
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
        /*
                else
                {
                        ClaimsPrincipal user = new(identity);
                AuthenticationState authenticationState = new(user);
                    identity = new ClaimsIdentity();
                }
                */
        //ClaimsPrincipal user = new(identity);
        //AuthenticationState authenticationState = new(user);
        var u = new AuthenticationState(new ClaimsPrincipal(new ClaimsIdentity()));
        return Task.FromResult(u);
    }

    public void NotifyUserChanged()
    {
        NotifyAuthenticationStateChanged(GetAuthenticationStateAsync());
    }
}
