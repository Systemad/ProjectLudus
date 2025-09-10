using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Npgsql;
using PhenX.EntityFrameworkCore.BulkInsert.PostgreSql;
using Shared.Twitch;
using TickerQ.Dashboard.DependencyInjection;
using TickerQ.DependencyInjection;
using SyncService;
using SyncService.Data;
using SyncService.Seed;

var builder = WebApplication.CreateBuilder(args);

builder.Services.Configure<TwitchOptions>(builder.Configuration.GetSection("Twitch"));

builder.Services.AddHttpClient(
    "IGDB",
    httpClient =>
    {
        httpClient.BaseAddress = new Uri("https://api.igdb.com/v4/");
        httpClient.DefaultRequestHeaders.Add("Accept", "application/json");
    }
);

builder.Services.AddScoped<ApiClient>();
builder.Services.AddScoped<CompanySeeder>();
builder.Logging.AddConsole();

var connectionString = "Host=localhost;Port=5433;Username=myuser;Password=mypassword;Database=ludusmain";
var dataSourceBuilder = new NpgsqlDataSourceBuilder(connectionString);

dataSourceBuilder.EnableParameterLogging();
//var loggerFactory = LoggerFactory.Create(log => log.AddConsole());
//options.UseLoggerFactory(loggerFactory);
dataSourceBuilder.UseNodaTime();
dataSourceBuilder.EnableDynamicJson();
var dataSource = dataSourceBuilder.Build();
builder.Services.AddDbContextPool<AppDbContext>(opt =>
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
using (var scope = app.Services.CreateScope())
{
    //var seeder = scope.ServiceProvider.GetRequiredService<GameSeeder>();
    //await seeder.PopulateGamesAsync(false, true, false);
    //var seeder = scope.ServiceProvider.GetRequiredService<CompanySeeder>();
    //await seeder.PopulateCompaniesAsync(true, false);
}

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

//app.UseTickerQ(TickerQStartMode.Manual);
//ITickerHost tickerHost = app.Services.GetRequiredService<ITickerHost>(); 
//tickerHost.Start();

await app.RunAsync();