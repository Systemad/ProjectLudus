using System.Text.Json.Serialization;
using Catalog.Features.Calendar;
using Catalog.Features.Companies;
using Catalog.Features.Events;
using Catalog.Features.Games;
using Catalog.Features.IGDB;
using Catalog.Features.Steam;
using Catalog.Middleware;
using Catalog.Queries;
using CmdScale.EntityFrameworkCore.TimescaleDB;
using Microsoft.AspNetCore.Http.Json;

namespace Catalog.Extensions;

public static class CatalogModuleExtensions
{
    public static WebApplicationBuilder AddCatalogModule(this WebApplicationBuilder builder)
    {
        builder.Services.ConfigureHttpJsonOptions(options =>
        {
            options.SerializerOptions.DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull;
            options.SerializerOptions.NumberHandling = JsonNumberHandling.Strict;
        });
        builder.Services.Configure<JsonOptions>(options =>
        {
            options.SerializerOptions.Converters.Add(new JsonStringEnumConverter());
            options.SerializerOptions.NumberHandling = JsonNumberHandling.Strict;
            options.SerializerOptions.MaxDepth = 256;
            options.SerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles;
        });
        builder.Services.AddOpenApi(options =>
        {
            options.OpenApiVersion = OpenApiSpecVersion.OpenApi3_1;
            options.AddOperationTransformer<RequiredParameterTransformer>();
            options.AddSchemaTransformer<RequiredSchemaTransformer>();
        });
        builder.Services.AddValidation();
        builder.Services.AddExceptionHandler<ApiExceptionHandler>();
        builder.Services.AddProblemDetails();
        builder.Services.AddDbContext<AppDbContext>(optionsBuilder =>
        {
            optionsBuilder.UseQueryTrackingBehavior(QueryTrackingBehavior.NoTracking);
            optionsBuilder
                .UseNpgsql(
                    builder.Configuration.GetConnectionString("catalogdb"),
                    np =>
                    {
                        np.CommandTimeout(30);
                        np.ConfigureDataSource(_ => { });
                        np.UseQuerySplittingBehavior(QuerySplittingBehavior.SplitQuery);
                    }
                )
                .UseSnakeCaseNamingConvention()
                .UseTimescaleDb();
        });
        builder.EnrichNpgsqlDbContext<AppDbContext>();
        builder.Services.AddOutputCache(options =>
        {
            options.DefaultExpirationTimeSpan = TimeSpan.FromHours(1);
            options.AddPolicy("DefaultCache", policy => policy.Expire(TimeSpan.FromHours(1)));
            options.AddPolicy("SteamCache", policy => policy.Expire(TimeSpan.FromMinutes(10)));
        });
        builder.Services.AddGamesServices();
        builder.Services.AddScoped<ICompanyService, CompanyService>();
        builder.Services.AddScoped<IEventService, EventService>();
        builder.Services.AddScoped<ISteamService, SteamService>();
        builder.Services.AddScoped<IIGDBService, IGDBService>();
        builder.Services.AddScoped<ICalendarService, CalendarService>();
        builder.Services.AddScoped<CatalogGameQueries>();

        return builder;
    }

    public static WebApplication MapCatalogModule(this WebApplication app)
    {
        app.MapCalendarFeature();
        app.MapGamesFeature();
        app.MapCompaniesFeature();
        app.MapEventsFeature();
        app.MapIGDBFeature();
        app.MapSteamFeature();

        return app;
    }
}
