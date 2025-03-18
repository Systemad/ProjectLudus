using System.Net.Http.Json;
using System.Text;
using Ludus.Data;
using Ludus.Seeder;
using Ludus.Shared;
using Marten;

var store = DocumentStore.For(
    "host=localhost:5432;database=doctesting;password=Compaq2009;username=dan1"
);
await using var session = store.QuerySession();

var game = await session.Query<Game>().Where(x => x.Id == 242408).FirstOrDefaultAsync();

var gamea = await session.Query<Game>().Where(g => g.Id == 302156).FirstOrDefaultAsync();

var queryablea = await session
    .Query<Game>()
    .Where(x => x.Id == 242408)
    .Select(x => x.Artworks)
    .FirstOrDefaultAsync();

//var hey = await session.LoadAsync<Game>(242408)
var heaa = 1;

/*
var httpClient = new HttpClient();
httpClient.BaseAddress = new Uri("https://api.igdb.com/v4/");
httpClient.DefaultRequestHeaders.Add("Client-ID", "i0s32q3oi8z074rvq0ljkaupnbkq98");
httpClient.DefaultRequestHeaders.Add("Authorization", "Bearer x8hmb9ft7j8mohy6yjfdm9ano95abm");
httpClient.DefaultRequestHeaders.Add("Accept", "application/json");

var response = await httpClient.PostAsync("games/count", null);
var countResponse = await response.Content.ReadFromJsonAsync<CountResponse>();

int maxItemsPerIteration = 500;
long totalItems = countResponse.Count;
long iterations = (totalItems + maxItemsPerIteration - 1) / maxItemsPerIteration;

for (long i = 0; i < iterations; i++)
{
    long offset = i * maxItemsPerIteration; // Correct offset calculation
    long itemsToTake = Math.Min(maxItemsPerIteration, totalItems - offset);

    await FetchAndStoreGamesAsync(itemsToTake, offset); // Fetch and insert into DB
    await Task.Delay(500); // Respect API rate limits
}

async Task FetchAndStoreGamesAsync(long itemsToTake, long offset)
{
    var requestBody =
        $"fields {string.Join(",", Query.Fields)}; limit {itemsToTake}; offset {offset};";
    using var request = new HttpRequestMessage(HttpMethod.Post, "games");
    request.Content = new StringContent(requestBody, Encoding.UTF8, "text/plain");

    var response = await httpClient.SendAsync(request);
    response.EnsureSuccessStatusCode();

    var games = await response.Content.ReadFromJsonAsync<List<Game>>();

    if (games == null || games.Count == 0)
        return;

    await store.BulkInsertAsync(
        games,
        BulkInsertMode.OverwriteExisting,
        batchSize: (int)itemsToTake
    );
}

*/
/*
await SeedDatabaseAsync();
return;

static async Task SeedDatabaseAsync()
{
    var httpClient = new HttpClient();
    httpClient.BaseAddress = new Uri("https://api.igdb.com/v4/");
    httpClient.DefaultRequestHeaders.Add("Client-ID", "i0s32q3oi8z074rvq0ljkaupnbkq98");
    httpClient.DefaultRequestHeaders.Add("Authorization", "Bearer x8hmb9ft7j8mohy6yjfdm9ano95abm");
    httpClient.DefaultRequestHeaders.Add("Accept", "application/json");

    var response = await httpClient.PostAsync("games/count", null);
    var countResponse = await response.Content.ReadFromJsonAsync<CountResponse>();

    int maxItemsPerIteration = 500;
    long totalItems = countResponse.Count;
    long iterations = (totalItems + maxItemsPerIteration - 1) / maxItemsPerIteration;

    for (long i = 0; i < iterations; i++)
    {
        long offset = i * maxItemsPerIteration; // Correct offset calculation
        long itemsToTake = Math.Min(maxItemsPerIteration, totalItems - offset);

        await FetchAndStoreGamesAsync(httpClient, itemsToTake, offset); // Fetch and insert into DB
        await Task.Delay(500); // Respect API rate limits
    }
}

static async Task FetchAndStoreGamesAsync(HttpClient httpClient, long itemsToTake, long offset)
{
    var requestBody =
        $"fields {string.Join(",", Query.Fields)}; limit {itemsToTake}; offset {offset};";
    using var request = new HttpRequestMessage(HttpMethod.Post, "games");
    request.Content = new StringContent(requestBody, Encoding.UTF8, "text/plain");

    var response = await httpClient.SendAsync(request);
    response.EnsureSuccessStatusCode();

    var games = await response.Content.ReadFromJsonAsync<List<Game>>();

    if (games == null || games.Count == 0)
        return;

    using var gamesDbContext = new GamesDbContext();
    gamesDbContext.Games.AddRange(games);
    await gamesDbContext.SaveChangesAsync();
}
*/
