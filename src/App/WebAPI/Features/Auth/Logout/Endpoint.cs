using FastEndpoints;
using FastEndpoints.Security;
using Microsoft.AspNetCore.Authentication.Cookies;

namespace Auth.Logout;

public class Endpoint : EndpointWithoutRequest<IResult>
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
