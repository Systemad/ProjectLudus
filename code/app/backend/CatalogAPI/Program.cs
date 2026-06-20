using System.Text.Json.Serialization;
using Data;
using CatalogAPI.Extensions;
using CatalogAPI.Features.Calendar;
using CatalogAPI.Features.Companies;
using CatalogAPI.Features.Events;
using CatalogAPI.Features.Games;
using CatalogAPI.Features.Homepage;
using CatalogAPI.Features.IGDB;
using CatalogAPI.Features.Steam;
using CmdScale.EntityFrameworkCore.TimescaleDB;
using Data.Context;
using Microsoft.AspNetCore.Http.Json;
using Scalar.AspNetCore;
// dotnet ef dbcontext scaffold --project CatalogAPI\CatalogAPI.csproj --startup-project CatalogAPI\CatalogAPI.csproj --configuration Debug --no-build "Host=localhost;Port=5433;Username=postgres;Password=pudGW.E7_u8eF8Qhnym)E0;Database=catalogdev" CmdScale.EntityFrameworkCore.TimescaleDB.Design --context AppDbContext --context-dir Context --force --output-dir Data --schema igdb --schema steam --no-onconfiguring

// dotnet ef dbcontext scaffold --startup-file App.cs --no-build "Host=65.109.131.166;Port=5433;Database=catalogdb;Username=casedan;Password=K5aIfDjpPLKXVR5UgwYG" CmdScale.EntityFrameworkCore.TimescaleDB.Design --file Modelss.cs --schema igdb --schema steam --no-onconfiguring
var builder = WebApplication.CreateBuilder(args);
    
builder.Services.ConfigureHttpJsonOptions(options =>
{
    options.SerializerOptions.DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull;
});
builder.Services.AddOpenApi(options =>
{
    options.AddOperationTransformer<RequiredParameterTransformer>();    
    options.AddSchemaTransformer<RequiredSchemaTransformer>();
});

builder.AddServiceDefaults();
builder.Services.Configure<JsonOptions>(options =>
{
    options.SerializerOptions.Converters.Add(new JsonStringEnumConverter());
    options.SerializerOptions.NumberHandling = JsonNumberHandling.Strict;
    options.SerializerOptions.MaxDepth = 256;
    options.SerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles;
});
builder.Services.ConfigureHttpJsonOptions(options =>
{
    options.SerializerOptions.NumberHandling = JsonNumberHandling.Strict;
});
builder.Services.AddValidation();
builder.Services.AddExceptionHandler<CatalogAPI.Middleware.ApiExceptionHandler>();
builder.Services.AddProblemDetails();
builder.Services.AddDbContext<AppDbContext>(optionsBuilder =>
{
    optionsBuilder.UseQueryTrackingBehavior(QueryTrackingBehavior.NoTracking);
    optionsBuilder.UseNpgsql(
        builder.Configuration.GetConnectionString("catalogdev"),
        np =>
        {
            np.CommandTimeout(30);
            np.ConfigureDataSource(ds =>
            {
            });
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
    options.AddPolicy(
        "DefaultCache",
        outputCachePolicyBuilder => outputCachePolicyBuilder.Expire(TimeSpan.FromHours(1))
    );
    options.AddPolicy(
        "SteamCache",
        outputCachePolicyBuilder => outputCachePolicyBuilder.Expire(TimeSpan.FromMinutes(10))
    );
});

builder.Services.AddGamesServices();
builder.Services.AddScoped<ICompanyService, CompanyService>();
builder.Services.AddScoped<IEventService, EventService>();
builder.Services.AddScoped<ISteamService, SteamService>();
builder.Services.AddScoped<IIGDBService, IGDBService>();
builder.Services.AddScoped<IHomepageService, HomepageService>();
builder.Services.AddScoped<ICalendarService, CalendarService>();

var app = builder.Build();

//app.MapDefaultEndpoints();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.MapScalarApiReference(
        "/docs",
        options =>
        {
            options.WithTitle("My API Documentation").ForceDarkMode();
            options.DisableAgent();
            options.DisableTelemetry();
        }
    );
}

app.UseExceptionHandler();
app.UseStatusCodePages();
app.UseHttpsRedirection();
app.UseOutputCache();

app.MapCalendarFeature();
app.MapGamesFeature();
app.MapCompaniesFeature();
app.MapEventsFeature();
app.MapIGDBFeature();
app.MapHomepageFeature();
app.MapSteamFeature();

app.Run();
