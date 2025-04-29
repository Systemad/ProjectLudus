using Ludus.Service;
using Ludus.Service.Twitch;
using Marten;
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
builder.Services.AddSingleton<ITwitchAccessTokenService, TwitchAccessTokenService>();
builder.Services.AddScoped<DataSeeder>();

builder.Services.AddMarten(options =>
{
    options.Connection("host=localhost:5432;database=gamingdb;password=Compaq2009;username=dan1");
    options.UseSystemTextJsonForSerialization();
    if (builder.Environment.IsDevelopment())
    {
        options.AutoCreateSchemaObjects = AutoCreate.All;
    }
});

//builder.Services.AddOpenApi();

var app = builder.Build();

/*
// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();
*/
using (var scope = app.Services.CreateScope())
{
    var store = scope.ServiceProvider.GetRequiredService<IDocumentStore>();

    /*
    await store.Advanced.Clean.CompletelyRemoveAllAsync();
    await store.Advanced.Clean.DeleteAllDocumentsAsync();
    await store.Advanced.Clean.DeleteAllEventDataAsync();

    var dataSeeder = scope.ServiceProvider.GetRequiredService<DataSeeder>();
    await dataSeeder.Populate();
    */
    var dataSeeder = scope.ServiceProvider.GetRequiredService<DataSeeder>();
    await using var session = store.LightweightSession();
    var gameTypes = await dataSeeder.FetchGamesTypesAsync();
    session.StoreObjects(gameTypes);
    await session.SaveChangesAsync();
}
app.Run();
