using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Npgsql;
using PhenX.EntityFrameworkCore.BulkInsert.PostgreSql;
using Shared.Twitch;
using TickerQ.Dashboard.DependencyInjection;
using TickerQ.DependencyInjection;
using SyncService.Data;
using SyncService.Features;
using SyncService.Features.Company;
using SyncService.Features.Games;
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
);

builder.Services.AddScoped<CompanyDatabaseService>();
builder.Services.AddScoped<GameDatabaseService>();

builder.Services.AddScoped<GameWebhookService>();

builder.Services.AddScoped<GameSeedingSeedingOrchestrator>();
builder.Services.AddScoped<CompanySeedingSeedingOrchestrator>();
builder.Logging.AddConsole();

var connectionString = "Host=localhost;Port=5433;Username=myuser;Password=mypassword;Database=ludusmain";
var dataSourceBuilder = new NpgsqlDataSourceBuilder(connectionString);

// TODO: disable in production
dataSourceBuilder.EnableParameterLogging();
dataSourceBuilder.UseNodaTime();
dataSourceBuilder.EnableDynamicJson();
var dataSource = dataSourceBuilder.Build();
builder.Services.AddDbContextPool<SyncDbContext>(opt =>
    opt.UseNpgsql(dataSource, o =>
        {
            o.SetPostgresVersion(17, 0);
            o.UseNodaTime();
        })
        .UseSnakeCaseNamingConvention()
        .UseBulkInsertPostgreSql()
);

builder.Services.AddTickerQ(options =>
{
    options.SetMaxConcurrency(4);
    options.AddDashboard();
    //options.SetExceptionHandler<MyExceptionHandler>(); 
});

var app = builder.Build();

// 7077
app.MapPost("/webhooks/games", async (string payload) =>
{
    //await handler.ProcessAsync(payload);
    return Results.Ok();
});


app.MapGet("/", async ([FromServices] NpgsqlConnection connection) =>
{
    await connection.OpenAsync();
    await using var command = new NpgsqlCommand("SELECT number FROM data LIMIT 1", connection);
    return "Hello World: " + await command.ExecuteScalarAsync();
});

app.UseTickerQ(TickerQStartMode.Immediate);
ITickerHost tickerHost = app.Services.GetRequiredService<ITickerHost>(); 
tickerHost.Start();

await app.RunAsync();