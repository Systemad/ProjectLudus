using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Npgsql;
using PhenX.EntityFrameworkCore.BulkInsert.PostgreSql;
using Shared.Features.Webhooks;
using Shared.Twitch;
using TickerQ.DependencyInjection;
using SyncService.Data;
using SyncService.Features;
using SyncService.Features.Companies;
using SyncService.Features.Games;
using SyncService.Features.Games.Webhook;
using SyncService.Utilities;
using SyncService.Workers;
using TickerQ.Utilities.Enums;
using TickerQ.Utilities.Interfaces;

var builder = WebApplication.CreateBuilder(args);

builder.AddServiceDefaults();
builder.Services.Configure<TwitchOptions>(builder.Configuration.GetSection("Twitch"));

builder.Services.AddScoped<ApiClient>();
builder.Services.AddHttpClient<ApiClient>(httpClient =>
    {
        httpClient.BaseAddress = new Uri("https://api.igdb.com/v4/");
        httpClient.DefaultRequestHeaders.Add("Accept", "application/json");
    }
).AddHttpMessageHandler<TwitchAuthenticationHandler>();

builder.Services.AddScoped<SeedingService>();
builder.Services.AddScoped<GameDatabaseService>();

builder.Services.AddScoped<GameWebhookProcessor>();

builder.Services.AddScoped<IgdbService>();
builder.Logging.AddConsole();

builder.AddNpgsqlDbContext<SyncDbContext>(connectionName: "syncdb", configureDbContextOptions: (optionsBuilder) =>
{
    optionsBuilder.UseQueryTrackingBehavior(QueryTrackingBehavior.NoTracking);
    optionsBuilder.UseNpgsql(np =>
    {
        //np.UseNodaTime();
        np.ConfigureDataSource(x =>
        {
            x.EnableParameterLogging();
            x.UseNodaTime();
            x.EnableDynamicJson();
        });
    });
    optionsBuilder.UseSnakeCaseNamingConvention();
    optionsBuilder.UseBulkInsertPostgreSql();
});

builder.Services.AddTickerQ(options =>
{
    options.SetMaxConcurrency(4);
    //options.AddDashboard();
    //options.SetExceptionHandler<MyExceptionHandler>(); 
});

builder.Services.AddHostedService<SyncWorker>();

var app = builder.Build();

// 7077
app.MapPost("/webhooks/create/games",
    async ([FromBody] WebhookGamePayload payload, [FromServices] GameWebhookProcessor webhookProcessor) =>
    {
        await webhookProcessor.ProcessWebhookEventAsync(payload.Id, WebhookMethod.CREATE);
    });
app.MapPost("/webhooks/update/games",
    async ([FromBody] WebhookGamePayload payload, [FromServices] GameWebhookProcessor webhookProcessor) =>
    {
        await webhookProcessor.ProcessWebhookEventAsync(payload.Id, WebhookMethod.UPDATE);
    });
app.MapPost("/webhooks/delete/games",
    async ([FromBody] WebhookDeleteGamePayload payload, [FromServices] GameWebhookProcessor webhookProcessor) =>
    {
        await webhookProcessor.ProcessWebhookEventAsync(payload.Id, WebhookMethod.DELETE);
    });

app.UseTickerQ(TickerQStartMode.Immediate);
ITickerHost tickerHost = app.Services.GetRequiredService<ITickerHost>();
tickerHost.Start();

await app.RunAsync();