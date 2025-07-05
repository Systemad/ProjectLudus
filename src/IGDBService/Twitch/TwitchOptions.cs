namespace IGDBService.Twitch;

public sealed class TwitchOptions
{
    public required string ClientId { get; set; } = string.Empty;
    public required string ClientSecret { get; set; } = string.Empty;
}
