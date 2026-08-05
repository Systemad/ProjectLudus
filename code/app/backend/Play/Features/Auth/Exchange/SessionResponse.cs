using Play.Features.Auth.User;
using Play.Features.Lists.Common.Dtos;

namespace Play.Features.Auth.Exchange;

public sealed record SessionResponse(
    string AccessToken,
    DateTimeOffset ExpiresAt,
    UserResponse User,
    ListSummaryResponse Wishlist
);
