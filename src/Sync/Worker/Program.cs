using Marten;
using Shared.Twitch;
using Weasel.Core;

var builder = WebApplication.CreateBuilder(args);

/*
long count = 0;

await foreach (var _ in File.ReadLinesAsync("Cache/gamefile.json"))
{
    count++;
}

Console.WriteLine($"Total entries: {count}");
*/
builder.Services.Configure<TwitchOptions>(builder.Configuration.GetSection("Twitch"));
builder.Services.AddHttpClient(
    "IGDB",
    httpClient =>
    {
        httpClient.BaseAddress = new Uri("https://api.igdb.com/v4/");
        httpClient.DefaultRequestHeaders.Add("Accept", "application/json");
    }
);


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
    var store = scope.ServiceProvider.GetRequiredService<IDocumentStore>();

    await store.Advanced.Clean.CompletelyRemoveAllAsync();
    await store.Advanced.Clean.DeleteAllDocumentsAsync();
    await store.Advanced.Clean.DeleteAllEventDataAsync();
*/

}
app.Run();
