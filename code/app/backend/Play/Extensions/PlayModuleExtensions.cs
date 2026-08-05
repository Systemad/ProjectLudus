using System.Text.Json.Serialization;
using AspNet.Security.OpenId.Steam;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi;
using Play.Features.Auth;
using Play.Features.Lists;
using Play.Infrastructure.Persistence;
using Play.Queries;
using SteamWebAPI2.Utilities;

namespace Play.Extensions;

public static class PlayModuleExtensions
{
    public static WebApplicationBuilder AddPlayModule(this WebApplicationBuilder builder)
    {
        builder.Services.ConfigureHttpJsonOptions(options =>
            options.SerializerOptions.Converters.Add(new JsonStringEnumConverter())
        );
        builder.Services.AddOpenApi(options =>
        {
            options.AddDocumentTransformer(
                (document, _, _) =>
                {
                    document.Components ??= new OpenApiComponents();
                    document.Components.SecuritySchemes ??=
                        new Dictionary<string, IOpenApiSecurityScheme>();
                    document.Components.SecuritySchemes["PlayBearer"] = new OpenApiSecurityScheme
                    {
                        Type = SecuritySchemeType.Http,
                        Scheme = "bearer",
                        BearerFormat = "opaque",
                    };
                    return Task.CompletedTask;
                }
            );
            options.AddOperationTransformer(
                (operation, context, _) =>
                {
                    var requiresAuthorization = context
                        .Description.ActionDescriptor.EndpointMetadata.OfType<IAuthorizeData>()
                        .Any();

                    if (!requiresAuthorization)
                        return Task.CompletedTask;

                    operation.Security =
                    [
                        new OpenApiSecurityRequirement
                        {
                            [new OpenApiSecuritySchemeReference("PlayBearer", context.Document)] =
                            [],
                        },
                    ];
                    return Task.CompletedTask;
                }
            );
        });
        builder.Services.AddDbContext<PlayDbContext>(optionsBuilder =>
        {
            optionsBuilder.UseQueryTrackingBehavior(QueryTrackingBehavior.NoTracking);
            optionsBuilder
                .UseNpgsql(
                    builder.Configuration.GetConnectionString("playdb"),
                    np =>
                    {
                        np.CommandTimeout(30);
                        np.ConfigureDataSource(_ => { });
                        np.UseQuerySplittingBehavior(QuerySplittingBehavior.SplitQuery);
                    }
                )
                .UseSnakeCaseNamingConvention();
        });
        builder.Services.AddScoped<AuthService>();
        builder.Services.AddScoped<ListService>();
        builder.Services.AddScoped<PlayHistoryQueries>();
        builder.Services.AddScoped<PlayListQueries>();
        builder.Services.AddScoped<PlayMembershipQueries>();
        builder.Services.AddHttpClient("steam");
        builder.Services.AddSingleton<SteamWebInterfaceFactory>(_ =>
        {
            var steamApiKey =
                builder.Configuration["Steam:ApiKey"]
                ?? throw new InvalidOperationException("Steam:ApiKey is required.");
            return new SteamWebInterfaceFactory(steamApiKey);
        });
        builder
            .Services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = "PlayBearer";
                options.DefaultChallengeScheme = SteamAuthenticationDefaults.AuthenticationScheme;
            })
            .AddScheme<AuthenticationSchemeOptions, PlayBearerHandler>("PlayBearer", _ => { })
            .AddCookie("SteamExternal")
            .AddSteam(options =>
            {
                options.ApplicationKey = builder.Configuration["Steam:ApiKey"];
                options.SignInScheme = "SteamExternal";
            });
        builder.Services.AddAuthorization();
        builder.Services.AddPlayApiRateLimiting();

        if (builder.Environment.IsDevelopment())
        {
            builder.Services.Configure<ForwardedHeadersOptions>(options =>
            {
                options.ForwardedHeaders =
                    ForwardedHeaders.XForwardedFor
                    | ForwardedHeaders.XForwardedHost
                    | ForwardedHeaders.XForwardedProto;
                options.KnownIPNetworks.Clear();
                options.KnownProxies.Clear();
            });
        }

        return builder;
    }

    public static WebApplication MapPlayModule(this WebApplication app)
    {
        app.MapAuthEndpoints();
        app.MapListEndpoints();

        return app;
    }
}
