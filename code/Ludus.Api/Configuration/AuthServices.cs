using FastEndpoints.Security;
using Microsoft.AspNetCore.Authentication.Cookies;
using Ludus.Api.Features.Auth;

namespace Ludus.Api.Configuration;

public static class AuthServices
{
    public static IServiceCollection AddAuth(this IServiceCollection services)
    {
        services
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
        return services;
    }
}
