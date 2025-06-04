using FastEndpoints;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;

namespace Ludus.Server.Features.Auth.Endpoints;

public class LoginAsync : EndpointWithoutRequest<IResult>
{
    public override void Configure()
    {
        Get("~/signin");
        AuthSchemes(CookieAuthenticationDefaults.AuthenticationScheme);
        AuthSchemes("Steam");
        AllowAnonymous();
    }

    public override async Task HandleAsync(CancellationToken ct)
    {
        await SendResultAsync(
            Results.Challenge(
                new AuthenticationProperties
                {
                    RedirectUri = "/Auth/steamresponse",
                    //IsPersistent = false,
                    //IssuedUtc = DateTime.Now,
                    //ExpiresUtc = DateTimeOffset.UtcNow.AddDays(7),
                },
                ["Steam"]
            )
        );
    }
}
