using CatalogAPI.Data;
using CatalogAPI.Features;
using CatalogAPI.Features.Games;
using CatalogAPI.Features.Games.Webhook;
using CatalogAPI.Seeding;
using CatalogAPI.Utilities;
using CatalogAPI.Workers;
using FastEndpoints;
using FastEndpoints.Swagger;
using IGDB;
using Microsoft.EntityFrameworkCore;
using Npgsql;
using PhenX.EntityFrameworkCore.BulkInsert.PostgreSql;
using Scalar.AspNetCore;
using Shared.Twitch;
using TickerQ.DependencyInjection;
using TickerQ.Utilities.Enums;
using TickerQ.Utilities.Interfaces;

var builder = WebApplication.CreateBuilder(args);

builder.AddServiceDefaults();

builder.AddNpgsqlDbContext<CatalogContext>(
    connectionName: "catalog-db",
    configureDbContextOptions: (optionsBuilder) =>
    {
        optionsBuilder.UseQueryTrackingBehavior(QueryTrackingBehavior.NoTracking);
        optionsBuilder.UseNpgsql(np =>
        {
            //np.MapEnum<IgdbReference>("type");
            np.UseNodaTime();
            np.ConfigureDataSource(x =>
            {
                x.EnableParameterLogging();
                x.UseNodaTime();
                //x.EnableDynamicJson();
            });
        });
        //optionsBuilder.UseSnakeCaseNamingConvention();
        //optionsBuilder.UseBulkInsertPostgreSql();
    }
);

builder.EnrichNpgsqlDbContext<CatalogContext>();

builder.Services.Configure<TwitchOptions>(builder.Configuration.GetSection("IGDB"));
builder.Logging.AddConsole();

    
var igdbClient = IGDBClient.CreateWithDefaults(
    Environment.GetEnvironmentVariable("IGDB_CLIENT_ID"),
    Environment.GetEnvironmentVariable("IGDB_CLIENT_SECRET")
);
builder.Services.AddSingleton<IGDBClient>(igdbClient);

builder
    .Services.AddFastEndpoints()
    //.AddIdempotency()
    .SwaggerDocument(options =>
    {
        options.DocumentSettings = s =>
        {
            s.Title = "Catalog API";
            s.Version = "v1";
        };
        options.ShortSchemaNames = true;
        options.DocumentSettings = s =>
        {
            s.MarkNonNullablePropsAsRequired();
        };
    });

builder.Services.AddHttpClient();
builder.Services.AddHttpContextAccessor();
builder.Services.AddControllers();

/*
builder.Services.AddScoped<ApiClient>();
builder
    .Services.AddHttpClient<ApiClient>(httpClient =>
    {
        httpClient.BaseAddress = new Uri("https://api.igdb.com/v4/");
        httpClient.DefaultRequestHeaders.Add("Accept", "application/json");
    })
    .AddHttpMessageHandler<TwitchAuthenticationHandler>();
*/
//builder.Services.AddScoped<SeedingService>();
//builder.Services.AddScoped<GameDatabaseService>();

//builder.Services.AddScoped<GameWebhookProcessor>();

//builder.Services.AddScoped<IgdbService>();
//builder.Logging.AddConsole();
/*
builder.Services.AddTickerQ(options =>
{
    options.SetMaxConcurrency(4);
    //options.AddDashboard();
    //options.SetExceptionHandler<MyExceptionHandler>();
});
*/
// builder.Services.AddHostedService<SyncWorker>();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseOpenApi(c => c.Path = "/openapi/v1.json");
    app.MapScalarApiReference();

    //app.MapOpenApi();
    app.Map("/scalar", () => Results.Redirect("/scalar/v1"));
}
app.UseDefaultExceptionHandler();
app.UseHttpsRedirection();
app.UseRouting();
app.UseFastEndpoints(x =>
    {
        x.Errors.UseProblemDetails();
        //x.Endpoints.ShortNames = true;
    })
    .UseSwaggerGen(c => { });

//app.UseTickerQ(TickerQStartMode.Immediate);
//ITickerHost tickerHost = app.Services.GetRequiredService<ITickerHost>();
//tickerHost.Start();

await app.RunAsync();
