namespace API.Features.TwitchAuth;

public interface ITwitchAccessTokenService
{
    string GetAccessToken();
    Task<TwitchBaseTokenResponse> FetchAndSetAccessTokenAsync();
}