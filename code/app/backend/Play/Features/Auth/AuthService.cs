using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using Microsoft.EntityFrameworkCore;
using Play.Features.Auth.Exchange;
using Play.Features.Auth.User;
using Play.Features.Lists.Common.Dtos;
using Play.Infrastructure.Persistence;
using SteamWebAPI2.Interfaces;
using SteamWebAPI2.Utilities;
using PlayUser = Play.Infrastructure.Persistence.User;

namespace Play.Features.Auth;

public sealed class AuthService(
    PlayDbContext db,
    SteamWebInterfaceFactory steamFactory,
    IHttpClientFactory clients
)
{
    public async Task<(PlayUser User, UserList Wishlist)> ProvisionAsync(
        ClaimsPrincipal principal,
        CancellationToken ct
    )
    {
        var claimedId =
            principal.FindFirstValue(ClaimTypes.NameIdentifier)
            ?? throw new InvalidOperationException("Steam did not return an identifier.");
        var steamId = new Uri(claimedId).Segments[^1];
        var steamName = principal.Identity?.Name;
        if (steamName is null)
            throw new InvalidOperationException("Steam did not return a display name.");
        var user = await db
            .Users.AsTracking()
            .Include(x => x.Lists)
            .FirstOrDefaultAsync(x => x.SteamId == steamId, ct);
        if (user is null)
        {
            user = new PlayUser { SteamId = steamId, SteamName = steamName };
            user.Lists.Add(
                new UserList
                {
                    Name = "Wishlist",
                    IsDefault = true,
                    Visibility = ListVisibility.Private,
                }
            );
            db.Users.Add(user);
        }
        else
        {
            user.SteamName = steamName;
            user.UpdatedAt = DateTimeOffset.UtcNow;
            if (user.Lists.All(x => !x.IsDefault))
            {
                user.Lists.Add(new UserList { Name = "Wishlist", IsDefault = true });
            }
        }
        await EnrichProfileAsync(user);
        await db.SaveChangesAsync(ct);
        return (user, user.Lists.Single(x => x.IsDefault));
    }

    public async Task<AuthExchangeResponse> CreateExchangeAsync(
        PlayUser user,
        string? state,
        CancellationToken ct
    )
    {
        var rawCode = CreateSecret();
        db.AuthTransactions.Add(
            new AuthTransaction
            {
                UserId = user.Id,
                CodeHash = Hash(rawCode),
                State = state,
                ExpiresAt = DateTimeOffset.UtcNow.AddMinutes(10),
            }
        );
        await db.SaveChangesAsync(ct);
        return new AuthExchangeResponse(rawCode, state);
    }

    public async Task<SessionResponse?> ExchangeAsync(
        string code,
        string? state,
        CancellationToken ct
    )
    {
        var transaction = await db
            .AuthTransactions.AsTracking()
            .Include(x => x.User)
                .ThenInclude(x => x!.Lists)
            .FirstOrDefaultAsync(
                x =>
                    x.CodeHash == Hash(code)
                    && x.ConsumedAt == null
                    && x.ExpiresAt > DateTimeOffset.UtcNow,
                ct
            );
        if (transaction?.User is null || transaction.State != state)
            return null;
        transaction.ConsumedAt = DateTimeOffset.UtcNow;
        var rawToken = CreateSecret();
        db.AuthSessions.Add(
            new AuthSession
            {
                UserId = transaction.User.Id,
                TokenHash = Hash(rawToken),
                ExpiresAt = DateTimeOffset.UtcNow.AddDays(30),
            }
        );
        await db.SaveChangesAsync(ct);
        var wishlist = transaction.User.Lists.Single(x => x.IsDefault);
        return new SessionResponse(
            rawToken,
            DateTimeOffset.UtcNow.AddDays(30),
            ToUser(transaction.User),
            ToList(wishlist)
        );
    }

    public async Task<PlayUser?> ValidateAsync(string token, CancellationToken ct)
    {
        var session = await db
            .AuthSessions.Include(x => x.User)
            .FirstOrDefaultAsync(
                x =>
                    x.TokenHash == Hash(token)
                    && x.RevokedAt == null
                    && x.ExpiresAt > DateTimeOffset.UtcNow,
                ct
            );
        return session?.User;
    }

    public async Task<UserResponse> GetUserAsync(Guid userId, CancellationToken ct)
    {
        var user = await db.Users.FirstAsync(x => x.Id == userId, ct);
        return ToUser(user);
    }

    public async Task RevokeAsync(string token, CancellationToken ct)
    {
        var session = await db
            .AuthSessions.AsTracking()
            .FirstOrDefaultAsync(x => x.TokenHash == Hash(token), ct);
        if (session is null)
            return;
        session.RevokedAt = DateTimeOffset.UtcNow;
        await db.SaveChangesAsync(ct);
    }

    public static string CreateSecret() =>
        Convert
            .ToBase64String(RandomNumberGenerator.GetBytes(32))
            .Replace("+", "-")
            .Replace("/", "_")
            .TrimEnd('=');

    public static string Hash(string value) =>
        Convert.ToHexString(SHA256.HashData(Encoding.UTF8.GetBytes(value)));

    public static UserResponse ToUser(PlayUser user)
    {
        if (user.SteamName is null)
            throw new InvalidOperationException("Steam profile is missing a display name.");
        if (user.AvatarUrl is null)
            throw new InvalidOperationException("Steam profile is missing an avatar.");
        return new(user.Id, user.SteamId, user.SteamName, user.AvatarUrl, user.Role);
    }

    public static ListSummaryResponse ToList(UserList list) =>
        new(
            list.Id,
            list.Name,
            list.Description,
            list.Visibility,
            list.IsDefault,
            list.Items.Count,
            list.CreatedAt,
            list.UpdatedAt
        );

    private async Task EnrichProfileAsync(PlayUser user)
    {
        var steam = steamFactory.CreateSteamWebInterface<SteamUser>(clients.CreateClient("steam"));
        var profile =
            (await steam.GetPlayerSummaryAsync(ulong.Parse(user.SteamId))).Data
            ?? throw new InvalidOperationException("Steam profile data was empty.");
        user.SteamName = profile.Nickname;
        user.AvatarUrl = profile.AvatarFullUrl;
    }
}
