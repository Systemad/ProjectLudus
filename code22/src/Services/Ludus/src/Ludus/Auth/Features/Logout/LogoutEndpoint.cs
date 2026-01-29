using FastEndpoints;
using FastEndpoints.Security;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Http;

namespace Ludus.Auth.Features.Logout;

public class LogoutEndpoint : EndpointWithoutRequest<IResult>
{
    public override void Configure()
    {
        Get("~/signout");
        AuthSchemes(CookieAuthenticationDefaults.AuthenticationScheme);
        AuthSchemes("Steam");
    }

    public override async Task HandleAsync(CancellationToken ct)
    {
        await CookieAuth.SignOutAsync();
    }
}
