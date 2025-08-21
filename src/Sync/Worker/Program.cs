using JasperFx;
using JasperFx.CodeGeneration;
using Marten;
using Shared;
using Shared.Twitch;
using TickerQ.Dashboard.DependencyInjection;
using TickerQ.DependencyInjection;
using TickerQ.Utilities.Enums;
using TickerQ.Utilities.Interfaces;
using Worker;
using Worker.Seed;

var builder = WebApplication.CreateBuilder(args);

builder.Host.ApplyJasperFxExtensions();
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
builder.Services.AddScoped<GameSeeder>();
builder.Services.AddScoped<CompanySeeder>();

var connection = builder.Configuration.GetConnectionString("DefaultConnection");
Console.WriteLine(connection);
builder.Services.AddNpgsqlDataSource(connection!);
builder.Services.AddMarten(options =>
    {
        options.Connection(connection!);
        options.Logger(new ConsoleMartenLogger());
        options.UseSystemTextJsonForSerialization();
        MartenSchema.Configure(options);
    })
    .UseNpgsqlDataSource()
    .ApplyAllDatabaseChangesOnStartup();

builder.Services.CritterStackDefaults(x =>
{
    x.Production.GeneratedCodeMode = TypeLoadMode.Static;
    x.Production.ResourceAutoCreate = AutoCreate.None;
});

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
    //await seeder.PopulateGamesAsync(true, false, true);
    //var seeder = scope.ServiceProvider.GetRequiredService<CompanySeeder>();
    //await seeder.PopulateCompaniesAsync(true, false);
}

app.MapPost("/webhooks/igdb", async (string payload, string handler) =>
{
    //await handler.ProcessAsync(payload);
    return Results.Ok();
});

app.UseTickerQ(TickerQStartMode.Manual);
//ITickerHost tickerHost = app.Services.GetRequiredService<ITickerHost>(); 
//tickerHost.Start();
return await app.RunJasperFxCommands(args);
