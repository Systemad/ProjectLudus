using System.Net.Http.Headers;
using BuildingBlocks.Twitch;

namespace Catalog.Worker.Utilities;

public class TwitchAuthenticationHandler(TwitchOptions twitchOptions) : DelegatingHandler
{
    private TwitchOptions _twitchOptions = twitchOptions;

    protected override async Task<HttpResponseMessage> SendAsync(
        HttpRequestMessage request,
        CancellationToken cancellationToken
    )
    {
        request.Headers.Add("Client-ID", $"{_twitchOptions.IGDB_CLIENT_ID}");
        request.Headers.Authorization = new AuthenticationHeaderValue(
            "Bearer",
            _twitchOptions.IGDB_CLIENT_SECRET
        );
        //request.Headers.Add("Authorization", $"Bearer {_twitchOptions.IGDB_CLIENT_SECRET}");

        return await base.SendAsync(request, cancellationToken);
    }
}
