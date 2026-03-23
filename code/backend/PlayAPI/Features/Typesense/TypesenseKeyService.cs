using Microsoft.EntityFrameworkCore;
using PlayAPI.Client.TypesenseClient;
using PlayAPI.Context;
using PlayAPI.Data;

namespace PlayAPI.Features.Typesense;

public class TypesenseKeyService
{
    private readonly ICreateKeyEndpoint _createKey;
    private readonly AppDbContext _db;

    public TypesenseKeyService(ICreateKeyEndpoint createKey, AppDbContext db)
    {
        _createKey = createKey;
        _db = db;
    }

    public async Task<string> GetValidKeyAsync()
    {
        var now = DateTime.UtcNow;
        var keyEntry = await _db.TypesenseKeys.FirstOrDefaultAsync();

        if (keyEntry == null || keyEntry.ExpiresAt <= now)
        {
            // Generate a new scoped search-only key
            var expiresAtUnix = DateTimeOffset.UtcNow.AddDays(7).ToUnixTimeSeconds();

            var keySchema = new ApiKeySchema
            {
                Description = "Search-only key for frontend",
                Actions = new[] { "search" },
                Collections = new[] { "games", "games" },
                ExpiresAt = expiresAtUnix
            };

            var createdKey = await _createKey.Execute(keySchema);

            if (keyEntry == null)
            {
                keyEntry = new TypesenseKey { Key = createdKey.Value, ExpiresAt = DateTimeOffset.FromUnixTimeSeconds(expiresAtUnix).UtcDateTime };
                _db.TypesenseKeys.Add(keyEntry);
            }
            else
            {
                keyEntry.Key = createdKey.Value;
                keyEntry.ExpiresAt = DateTimeOffset.FromUnixTimeSeconds(expiresAtUnix).UtcDateTime;
            }

            await _db.SaveChangesAsync();
        }

        return keyEntry.Key;
    }
}