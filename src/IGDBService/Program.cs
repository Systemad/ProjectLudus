using IGDBService;
using IGDBService.Twitch;
using Marten;
using Shared.Features.Games;
using Weasel.Core;

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

//builder.Services.AddSingleton<ITwitchAccessTokenService, TwitchAccessTokenService>();
builder.Services.AddScoped<Seeder>();

builder.Services.AddMarten(options =>
{
    options.Connection("host=localhost:5432;database=gamingdb;password=Compaq2009;username=dan1");
    options.UseSystemTextJsonForSerialization();
    if (builder.Environment.IsDevelopment())
    {
        options.AutoCreateSchemaObjects = AutoCreate.All;
    }
});

var app = builder.Build();
using (var scope = app.Services.CreateScope())
{
    /*
        await store.Advanced.Clean.CompletelyRemoveAllAsync();
        await store.Advanced.Clean.DeleteAllDocumentsAsync();
        await store.Advanced.Clean.DeleteAllEventDataAsync();
    
        var dataSeeder = scope.ServiceProvider.GetRequiredService<DataSeeder>();
        await dataSeeder.Populate();
    */
}
app.Run();
