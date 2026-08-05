using System.Security.Claims;
using System.Text.Encodings.Web;
using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.Options;

namespace Play.Features.Auth;

public sealed class PlayBearerHandler(
    IOptionsMonitor<AuthenticationSchemeOptions> options,
    ILoggerFactory logger,
    UrlEncoder encoder,
    AuthService auth
) : AuthenticationHandler<AuthenticationSchemeOptions>(options, logger, encoder)
{
    protected override async Task<AuthenticateResult> HandleAuthenticateAsync()
    {
        var value = Request.Headers.Authorization.ToString();
        if (!value.StartsWith("Bearer ", StringComparison.OrdinalIgnoreCase))
            return AuthenticateResult.NoResult();
        var user = await auth.ValidateAsync(value[7..].Trim(), Context.RequestAborted);
        if (user is null)
            return AuthenticateResult.Fail("Invalid or expired session.");
        var identity = new ClaimsIdentity(Scheme.Name);
        identity.AddClaims([
            new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
            new Claim("steam_id", user.SteamId),
            new Claim(ClaimTypes.Name, user.SteamName ?? user.SteamId),
            new Claim(ClaimTypes.Role, user.Role),
        ]);
        return AuthenticateResult.Success(
            new AuthenticationTicket(new ClaimsPrincipal(identity), Scheme.Name)
        );
    }
}
