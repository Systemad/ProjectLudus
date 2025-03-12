namespace Ludus.Server.Features.Twitch;

public interface ITwitchAccessTokenService
{
    string GetAccessToken();
    Task<TwitchBaseTokenResponse> FetchAndSetAccessTokenAsync();
}