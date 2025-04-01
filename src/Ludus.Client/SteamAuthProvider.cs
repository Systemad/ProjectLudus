using System.Security.Claims;
using Ludus.Shared;
using Ludus.Shared.Features.User;
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

    private static IEnumerable<Claim> GetClaims(User userInfo)
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
        /*
        authenticated = false;
        var user = unauthenticated;
        try
        {
             var userInfo = _httpClient.GetAsync("/api/me")

             if (userInfo != null)
             {
                 
             }
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
        */
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
