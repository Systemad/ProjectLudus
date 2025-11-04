using IGDB;

namespace Admin.FetchResource;

public class CustomTokenStore : ITokenStore
{
    private TwitchAccessToken _token;

    public Task<TwitchAccessToken> GetTokenAsync()
    {
        throw new NotImplementedException();
    }

    public Task<TwitchAccessToken> StoreTokenAsync(TwitchAccessToken token)
    {
        throw new NotImplementedException();
    }
}
