namespace API.Features.TwitchAuth;

public class TwitchOptions
{
    //public const string Twitch = "Twitch";
    public required string ClientId { get; set; } = string.Empty;
    public required string ClientSecret { get; set; } = string.Empty;
    //public string AccessToken { get; set; } = string.Empty;
}