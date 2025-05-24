using Ludus.Server.Features.Auth;
using Microsoft.AspNetCore.Authentication.Cookies;

namespace Ludus.Server.Configuration;

public static class AuthServices
{
    public static IServiceCollection AddAuth(this IServiceCollection services)
    {
        services
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
