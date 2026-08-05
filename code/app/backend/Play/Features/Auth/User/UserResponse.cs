namespace Play.Features.Auth.User;

public sealed record UserResponse(
    Guid Id,
    string SteamId,
    string SteamName,
    string AvatarUrl,
    string Role
);
