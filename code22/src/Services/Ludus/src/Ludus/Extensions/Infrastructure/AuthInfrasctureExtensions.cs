using FastEndpoints.Security;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;

namespace Ludus.Extensions.Infrastructure;

public static class AuthInfrasctureExtensions
{
    public static WebApplicationBuilder AddAuthInfrastucture(this WebApplicationBuilder builder)
    {
        builder.Services
            .AddAuthenticationCookie(validFor: TimeSpan.FromDays(7))
            .AddAuthorization()
            .AddAuthentication(options =>
            {
                options.DefaultScheme = CookieAuthenticationDefaults.AuthenticationScheme;
                //options.DefaultChallengeScheme = "Steam";
            })
            .AddCookie(options =>
            {
                options.LoginPath = "/signin";
                options.LogoutPath = "/signout";
                options.AccessDeniedPath = "/";
                options.ExpireTimeSpan = TimeSpan.FromDays(7);
            })
            .AddSteam();
        return builder;
    }

    public static WebApplication UseAuthInfrastucture(this WebApplication app)
    {
        app.UseAuthentication();
        app.UseAuthorization();
        app.UseCookiePolicy();
        return app;
    }
}
