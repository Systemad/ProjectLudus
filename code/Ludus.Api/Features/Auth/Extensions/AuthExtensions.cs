using Microsoft.AspNetCore.Authentication.Cookies;

namespace Ludus.Api.Features.Auth.Extensions;

// Credits: https://github.com/FastEndpoints/FastEndpoints/blob/main/Src/Security/AuthExtensions.cs
// Modified to add Steam authentication
public static class AuthExtensions
{
    /// <summary>
    /// configure and enable cookie based authentication
    /// </summary>
    /// <param name="validFor">specify how long the created cookie is valid for with a <see cref="TimeSpan" /></param>
    /// <param name="options">optional action for configuring cookie authentication options</param>
    public static IServiceCollection AddAuthenticationCookie(
        this IServiceCollection services,
        TimeSpan validFor,
        Action<CookieAuthenticationOptions>? options = null
    )
    {
        services
            .AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
            .AddCookie(o =>
            {
                //don't set Cookie.Expiration and Cookie.MaxAge here.
                //allow CookieAuthenticationHandler to take care of setting it depending on IsPersistent.
                //if we set it here, 'IsPersistent = false' won't have any effect.
                o.Cookie.Expiration = o.Cookie.MaxAge = null;
                o.ExpireTimeSpan = validFor;
                o.Cookie.HttpOnly = true;
                o.Cookie.SameSite = SameSiteMode.Lax;

                o.LoginPath = "/signin";
                o.LogoutPath = "/signout";
                o.AccessDeniedPath = "/";
                //o.ExpireTimeSpan = TimeSpan.FromDays(7);

                // ReSharper disable once ArrangeObjectCreationWhenTypeNotEvident
                o.Events = new CookieAuthenticationEvents
                {
                    OnRedirectToLogin = ctx =>
                    {
                        ctx.Response.Headers.Location = ctx.RedirectUri;
                        ctx.Response.StatusCode = 401;

                        return Task.CompletedTask;
                    },
                    OnRedirectToAccessDenied = ctx =>
                    {
                        ctx.Response.Headers.Location = ctx.RedirectUri;
                        ctx.Response.StatusCode = 403;

                        return Task.CompletedTask;
                    },
                    OnSigningIn = ctx =>
                    {
                        if (ctx.Properties.IsPersistent)
                            ctx.CookieOptions.MaxAge =
                                ctx.Properties.ExpiresUtc?.UtcDateTime - DateTime.UtcNow
                                ?? ctx.Options.ExpireTimeSpan;

                        return Task.CompletedTask;
                    },
                };
                options?.Invoke(o);
            })
            .AddSteam();

        return services;
    }
}
