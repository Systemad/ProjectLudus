using Microsoft.EntityFrameworkCore;
using PlayAPI.Client.TypesenseClient;
using PlayAPI.Context;
using PlayAPI.Data;

namespace PlayAPI.Features.Typesense.GetKey;

public class TypesenseKeyService
{
    public const string SearchCollection = "search___games_search";

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
        var keyEntry = await _db
            .TypesenseKeys.AsTracking()
            .OrderBy(x => x.Id)
            .FirstOrDefaultAsync();

        if (keyEntry is null || keyEntry.ExpiresAt <= now)
        {
            var expiresAtUnix = DateTimeOffset.UtcNow.AddDays(7).ToUnixTimeSeconds();

            var keySchema = new ApiKeySchema
            {
                Description = "Search-only key for frontend",
                Actions = new[] { "search" },
                Collections = new[] { SearchCollection },
                ExpiresAt = expiresAtUnix,
            };

            var createdKey = await _createKey.Execute(keySchema);

            if (keyEntry is null)
            {
                keyEntry = new TypesenseKey
                {
                    Key = createdKey.Value,
                    ExpiresAt = DateTimeOffset.FromUnixTimeSeconds(expiresAtUnix).UtcDateTime,
                };
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
