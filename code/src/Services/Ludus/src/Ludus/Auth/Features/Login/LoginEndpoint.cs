using FastEndpoints;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Http;

namespace Ludus.Auth.Features.Login;

public class LoginEndpoint : EndpointWithoutRequest<IResult>
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
        await Send.ResultAsync(
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
