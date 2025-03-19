namespace Ludus.Service.Twitch;

public interface ITwitchAccessTokenService
{
    Task<string> FetchAccessTokenAsync();
    TwitchOptions GetCredentials();
}
