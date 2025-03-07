using System.Text;
using System.Text.Json.Serialization;
using API.Features.TwitchAuth;
using Shared;

namespace API.Features.IGDB;

public class CountResponse
{
    [JsonPropertyName("count")] public long Count { get; set; }
}

public class FetchAndSeedInitialDatabase : IHostedService
{
    private IHttpClientFactory _httpClientFactory;
    private readonly IConfiguration _config;
    private TwitchOptions _twitchOptions;

    public FetchAndSeedInitialDatabase(IHttpClientFactory httpClientFactory)
    {
        _httpClientFactory = httpClientFactory;
        _config = new ConfigurationBuilder()
            .AddJsonFile("appsettings.json")
            .AddEnvironmentVariables()
            .Build();
    }

    public async Task StartAsync(CancellationToken cancellationToken)
    {
        _twitchOptions = _config.GetRequiredSection("Twitch").Get<TwitchOptions>();
        using HttpClient client = _httpClientFactory.CreateClient();

        client.BaseAddress = new Uri("https://api.igdb.com/v4/");
        client.DefaultRequestHeaders.Add("Client-ID", _twitchOptions.ClientId);
        client.DefaultRequestHeaders.Add("Authorization", "Bearer ");
        client.DefaultRequestHeaders.Add("Accept", "application/json");

        using var response = await client.PostAsync("games/count", null, cancellationToken);
        var data = await response.Content.ReadFromJsonAsync<CountResponse>(cancellationToken: cancellationToken);

        const int maxItemsPerIteration = 500;
        long totalItems = data.Count;
        long iterations = (totalItems + maxItemsPerIteration - 1) / maxItemsPerIteration;
        long offset = 0;
        long numIterations = 0;

        for (long i = 0; i < iterations; i++)
        {
            long itemsToTake = Math.Min(
                maxItemsPerIteration,
                totalItems - i * maxItemsPerIteration
            );
            offset += itemsToTake;
            numIterations++;
            await SetupGames(itemsToTake, offset);
            await Task.Delay(500, cancellationToken);
        }
    }

    private async Task SetupGames(long itemsToTake, long offset)
    {
        using HttpClient client = _httpClientFactory.CreateClient();

        var url = "https://api.igdb.com/v4/games";

        var body =
            $"fields {string.Join(",", Query.Fields)}; limit {itemsToTake}; offset {offset};";
        var body2 =
            $"fields *,\nage_ratings.*,\nage_ratings.rating_category.rating,\nage_ratings.organization.name,\nage_ratings.content_descriptions.*,\nalternative_names.*,\nartworks.image_id,\nartworks.url,\nbundles.*,\nbundles.cover.image_id,\ncollections.name,\ncollections.slug,\ncollections.url,\ncover.image_id,\ncreated_at,\nfirst_release_date,\nexternal_games.*,\nfranchise.name,\ngame_engines.*,\ngame_localizations.name,\ngame_localizations.region,\ngame_modes.name,\ngame_type.type,\ngenres.name,\ninvolved_companies.company.logo.*,\ninvolved_companies.company.name,\ninvolved_companies.company.slug,\ninvolved_companies.developer,\ninvolved_companies.publisher,\nkeywords.*,\nlanguage_supports.language.name,\nname,\nplatforms.name,\nplatforms.slug,\nplayer_perspectives.name,\nports.*,\nports.cover.*,\nrelease_dates.date,\nrelease_dates.platform,\nremakes.category,\nremakes.cover.image_id,\nremasters.category,\nremasters.cover.image_id,\nscreenshots.image_id,\nsimilar_games.category,\nsimilar_games.cover.image_id,\nsimilar_games.id,\nsimilar_games.name,\nsimilar_games.slug,\nslug,\nsummary,\nthemes.*,\nvideos.video_id,\nwebsites.category,\nwebsites.url,\nparent_game.*,\nparent_game.cover.*,\nrating;; limit {itemsToTake}; offset {offset};";
        var request = new StringContent(body, Encoding.UTF8, "application/json");

        client.DefaultRequestHeaders.Add("Client-ID", _twitchOptions.ClientId);
        client.DefaultRequestHeaders.Add("Authorization", "Bearer ");
        client.DefaultRequestHeaders.Add("Accept", "application/json");

        using var response = await client.PostAsync(url, request);
        var data2 = await response.Content.ReadAsStringAsync();
        var data = await response.Content.ReadFromJsonAsync<Game[]>();
        using (var dbContext = new AppDbContext())
        {
            dbContext.Games.AddRange(data);
            await dbContext.SaveChangesAsync();
        }
    }

    public Task StopAsync(CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }
}