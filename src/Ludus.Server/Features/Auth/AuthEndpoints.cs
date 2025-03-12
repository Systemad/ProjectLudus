using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;

namespace Ludus.Server.Features.Auth;

public static class AuthEndpoints
{
    public static IEndpointRouteBuilder MapAuthEndpoints(this IEndpointRouteBuilder routes)
    {
        
        routes.MapGet("~/signout", SignOutAsync);
        routes.MapGet("~/signin", SignIn);
        routes.MapPost("~/signin", SignPost);

        return routes;
    }

    private static Task<IResult> SignIn(HttpContext httpContext, [FromQuery] string returnUrl = "/")
    {
        return Task.FromResult(Results.Challenge(new AuthenticationProperties
        {
            RedirectUri = returnUrl,
            IsPersistent = true,
            IssuedUtc = DateTime.Now,
            ExpiresUtc = DateTimeOffset.UtcNow.AddDays(7)
        }, ["Steam"]));
    }
    
    private static Task<IResult> SignPost(HttpContext httpContext, [FromQuery] string returnUrl = "/")
    {
        return Task.FromResult(Results.Challenge(new AuthenticationProperties
        {
            RedirectUri = returnUrl,
            IsPersistent = true,
            IssuedUtc = DateTime.Now,
            ExpiresUtc = DateTimeOffset.UtcNow.AddDays(7)
        }, ["Steam"]));
    }
    
    private static async Task<IResult> SignOutAsync(HttpContext httpContext, [FromQuery] string returnUrl = "/")
    {
        await httpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);

        return Results.Redirect(returnUrl);
    }
}