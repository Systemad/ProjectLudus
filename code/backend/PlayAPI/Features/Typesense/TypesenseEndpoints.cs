using Microsoft.EntityFrameworkCore;
using PlayAPI.Context;
using PlayAPI.Data;

namespace PlayAPI.Features.Typesense;

public static class TypesenseEndpoints
{
    public static IEndpointRouteBuilder UseTagEndpoints(this IEndpointRouteBuilder routeBuilder)
    {
        var group = routeBuilder.MapGroup("/api");

        group.MapGet("/typesense-key", async (AppDbContext db, IConfiguration config) =>
        {
            // Get current key from DB
            var keyEntry = await db.TypesenseKeys.FirstOrDefaultAsync();

            var now = DateTime.UtcNow;

            if (keyEntry == null || keyEntry.ExpiresAt <= now)
            {
                var masterKey = config["TYPESENSE_MASTER_KEY"] ?? throw new Exception("Master key missing");
                var newKey = Guid.NewGuid().ToString(); 
                var expiresAt = now.AddDays(1); 

                if (keyEntry == null)
                {
                    keyEntry = new TypesenseKey { Key = newKey, ExpiresAt = expiresAt };
                    db.TypesenseKeys.Add(keyEntry);
                }
                else
                {
                    keyEntry.Key = newKey;
                    keyEntry.ExpiresAt = expiresAt;
                }

                await db.SaveChangesAsync();
            }

            return Results.Ok(new
            {
                Key = keyEntry.Key,
                ExpiresAt = keyEntry.ExpiresAt
            });
        });
        return group;
    }
}