using System.Security.Claims;
using Microsoft.AspNetCore.Components.Authorization;

namespace Ludus.Client;

public class SteamAuthProvider : AuthenticationStateProvider
{
    private readonly AuthenticatedUserService _userService;

    public SteamAuthProvider(AuthenticatedUserService userService)
    {
        _userService = userService;
    }

    private static IEnumerable<Claim> CreateClaimsPrincipal(UserDto userInfo)
    {
        return new[]
        {
            new Claim(ClaimTypes.Name, userInfo.Name),
            new Claim(ClaimTypes.NameIdentifier, userInfo.Id.ToString()),
            new Claim("SteamId", userInfo.SteamId),
            new Claim(ClaimTypes.Role, userInfo.Role),
        };
    }

    public override async Task<AuthenticationState> GetAuthenticationStateAsync()
    {
        if (_userService.User?.Claims.Count() > 0)
            return new AuthenticationState(_userService.User);

        ClaimsIdentity identity = new ClaimsIdentity();
        try
        {
            if (_userService.UserModel == null)
            {
                await _userService.FetchUserModelAsync();
            }

            if (_userService.UserModel == null)
            {
                _userService.User = new ClaimsPrincipal();
                return new AuthenticationState(_userService.User);
            }
            Console.WriteLine("user mode != null");
            IEnumerable<Claim> claims = CreateClaimsPrincipal(_userService.UserModel);
            identity = new ClaimsIdentity(claims, "Steam");
        }
        catch (HttpRequestException e)
        {
            Console.WriteLine(e);
            //throw;
        }

        _userService.User = new ClaimsPrincipal(identity);
        return new AuthenticationState(_userService.User);
    }
}
