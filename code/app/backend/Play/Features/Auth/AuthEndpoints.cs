using System.Security.Claims;
using AspNet.Security.OpenId.Steam;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http.HttpResults;
using Play.Features.Auth.Exchange;
using Play.Features.Auth.User;

namespace Play.Features.Auth;

public static class AuthEndpoints
{
    public static void MapAuthEndpoints(this IEndpointRouteBuilder endpoints)
    {
        endpoints
            .MapGet(
                "/auth/steam/login",
                (
                    HttpContext context,
                    string? state,
                    string? returnUrl,
                    IHostEnvironment environment
                ) =>
                {
                    var properties = new AuthenticationProperties
                    {
                        RedirectUri = "/auth/steam/callback",
                    };
                    if (!string.IsNullOrWhiteSpace(state))
                        properties.Items["state"] = state;
                    if (IsAllowedMobileReturnUrl(returnUrl, environment))
                        properties.Items["returnUrl"] = returnUrl!;
                    return TypedResults.Challenge(
                        properties,
                        [SteamAuthenticationDefaults.AuthenticationScheme]
                    );
                }
            )
            .WithTags("Auth")
            .AllowAnonymous();

        endpoints
            .MapGet(
                "/auth/steam/callback",
                async Task<Results<UnauthorizedHttpResult, RedirectHttpResult>> (
                    HttpContext context,
                    AuthService auth,
                    IHostEnvironment environment,
                    CancellationToken ct
                ) =>
                {
                    var result = await context.AuthenticateAsync("SteamExternal");
                    if (!result.Succeeded || result.Principal is null)
                        return TypedResults.Unauthorized();
                    var (user, _) = await auth.ProvisionAsync(result.Principal, ct);
                    await context.SignOutAsync("SteamExternal");
                    string? state = null;
                    result.Properties?.Items.TryGetValue("state", out state);
                    var exchange = await auth.CreateExchangeAsync(user, state, ct);
                    string? returnUrl = null;
                    result.Properties?.Items.TryGetValue("returnUrl", out returnUrl);
                    if (!IsAllowedMobileReturnUrl(returnUrl, environment))
                        returnUrl = "gameindex://auth/callback";
                    var values = new Dictionary<string, string?> { ["code"] = exchange.Code };
                    if (exchange.State is not null)
                        values.Add("state", exchange.State);
                    var target = QueryString.Create(values);
                    return TypedResults.Redirect(returnUrl + target);
                }
            )
            .WithTags("Auth")
            .AllowAnonymous();

        endpoints
            .MapPost(
                "/auth/mobile/exchange",
                async Task<Results<UnauthorizedHttpResult, Ok<SessionResponse>>> (
                    ExchangeRequest request,
                    AuthService auth,
                    CancellationToken ct
                ) =>
                {
                    var response = await auth.ExchangeAsync(request.Code, request.State, ct);
                    return response is null
                        ? TypedResults.Unauthorized()
                        : TypedResults.Ok(response);
                }
            )
            .WithTags("Auth")
            .Produces<SessionResponse>()
            .Produces(StatusCodes.Status401Unauthorized)
            .AllowAnonymous();

        endpoints
            .MapPost(
                "/auth/logout",
                async Task<NoContent> (HttpContext context, AuthService auth, CancellationToken ct) =>
                {
                    var value = context.Request.Headers.Authorization.ToString();
                    if (value.StartsWith("Bearer ", StringComparison.OrdinalIgnoreCase))
                    {
                        await auth.RevokeAsync(value[7..].Trim(), ct);
                    }
                    return TypedResults.NoContent();
                }
            )
            .WithTags("Auth")
            .Produces(StatusCodes.Status204NoContent)
            .RequireAuthorization();

        endpoints
            .MapGet(
                "/auth/me",
                async Task<Ok<UserResponse>> (
                    HttpContext context,
                    AuthService auth,
                    CancellationToken ct
                ) =>
                {
                    var userId = Guid.Parse(
                        context.User.FindFirstValue(ClaimTypes.NameIdentifier)
                            ?? throw new InvalidOperationException(
                                "Authenticated user is missing an identifier."
                            )
                    );
                    return TypedResults.Ok(await auth.GetUserAsync(userId, ct));
                }
            )
            .WithTags("Auth")
            .Produces<UserResponse>()
            .Produces(StatusCodes.Status401Unauthorized)
            .RequireAuthorization();
    }

    private static bool IsAllowedMobileReturnUrl(string? value, IHostEnvironment environment)
    {
        if (!Uri.TryCreate(value, UriKind.Absolute, out var uri))
            return false;

        return uri.Scheme.Equals("gameindex", StringComparison.OrdinalIgnoreCase)
            || (
                environment.IsDevelopment()
                && uri.Scheme.Equals("exp", StringComparison.OrdinalIgnoreCase)
            );
    }
}
