using IGDBService;
using JasperFx;
using Marten;
using Shared;
using Shared.Twitch;

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
builder.Services.AddScoped<GameSeeder>();
builder.Services.AddScoped<CompanySeeder>();

var connection = Utils.GetConnectionString();
builder.Services.AddNpgsqlDataSource(connection!);
builder.Services.AddMarten(options =>
    {
       
        options.Connection(connection!);
        
        options.UseSystemTextJsonForSerialization();
        options.AutoCreateSchemaObjects = AutoCreate.All;
        
        MartenSchema.Configure(options);
    })
    .UseNpgsqlDataSource()
    .ApplyAllDatabaseChangesOnStartup();

var app = builder.Build();
using (var scope = app.Services.CreateScope())
{
    //var seeder = scope.ServiceProvider.GetRequiredService<SeederService>();
    //await seeder.PopulateGamesAsync(true, true);
    //var seeder = scope.ServiceProvider.GetRequiredService<CompanySeeder>();
    //await seeder.PopulateCompaniesAsync(true, false);
}

app.Run();