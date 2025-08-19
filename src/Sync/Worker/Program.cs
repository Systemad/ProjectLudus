using JasperFx;
using JasperFx.CodeGeneration;
using Marten;
using Shared;
using Shared.Twitch;
using Worker;

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

var connection = Utilities.GetConnectionString();
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

var app = builder.Build();
using (var scope = app.Services.CreateScope())
{

    //var seeder = scope.ServiceProvider.GetRequiredService<GameSeeder>();
    //await seeder.PopulateGamesAsync(true, false, true);
    //var seeder = scope.ServiceProvider.GetRequiredService<CompanySeeder>();
    //await seeder.PopulateCompaniesAsync(true, false);
}

return await app.RunJasperFxCommands(args);
