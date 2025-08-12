using System.Linq.Expressions;
using Marten;
using NpgsqlTypes;
using Shared.Features;
using Shared.Features.Games;
using Weasel.Postgresql.Tables;

namespace Shared;

public class MartenSchema
{
    public static void Configure(StoreOptions options)
    {
        options.Schema.For<InsertIGDBGame>()
            .Index(x => x.Id, configure: idx => { idx.IsUnique = true; })
            .Index(x => x.Name)
            .Index(x => x.Slug)
            .Index(x => x.FirstReleaseDate)
            .Duplicate(x => x.TotalRating, configure: idx =>
            {
                idx.Method = IndexMethod.btree;
            })
            .Duplicate(x => x.TotalRatingCount, configure: idx =>
            {
                idx.Method = IndexMethod.btree;
            })
            .Duplicate(x => x.GameType.Id, configure: idx =>
            {
                idx.Method = IndexMethod.btree;
            })
            .Index(x => x.UpdatedAt)
            .Duplicate(x => x.Genres)
            .Duplicate(x => x.Platforms)
            .Duplicate(x => x.GameModes)
            .Index(x => new
                {
                    x.GameType.Id, x.TotalRatingCount, x.TotalRating
                }, idx =>
            {
                idx.Name = "idx_gametype_totalratingcount_totalrating";
            });
    }
}
