using System.Text.Json;
using Microsoft.AspNetCore.WebUtilities;

namespace Backend.API.Features.UserLibrary;

internal static class UserLibraryCursorScopes
{
    public const string History = "history";
    public const string ListGames = "list-games";
}

internal sealed record UserLibraryCursor(string Scope, DateTimeOffset Timestamp, long Id);

internal static class UserLibraryCursors
{
    public static string Encode(string scope, DateTimeOffset timestamp, long id) =>
        WebEncoders.Base64UrlEncode(
            JsonSerializer.SerializeToUtf8Bytes(new UserLibraryCursor(scope, timestamp, id))
        );

    public static bool TryDecode(string value, string expectedScope, out UserLibraryCursor? cursor)
    {
        try
        {
            cursor = JsonSerializer.Deserialize<UserLibraryCursor>(
                WebEncoders.Base64UrlDecode(value)
            );
            if (cursor is not null && cursor.Scope == expectedScope)
                return true;

            cursor = null;
            return false;
        }
        catch (FormatException)
        {
            cursor = null;
            return false;
        }
        catch (JsonException)
        {
            cursor = null;
            return false;
        }
    }
}
