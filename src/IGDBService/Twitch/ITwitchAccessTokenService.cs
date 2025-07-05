namespace IGDBService.Twitch;

public interface ITwitchAccessTokenService
{
    Task<string> FetchAccessTokenAsync();
    TwitchOptions GetCredentials();
}
