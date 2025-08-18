using Marten;
using Shared.Features;
using Weasel.Postgresql.Tables;

namespace Shared;

public static class MartenSchema
{
    public static void Configure(StoreOptions options)
    {
        options.Schema.For<IGDBGameFlat>().FullTextIndex(x => x.Name, x => x.AlternativeNames);

        options.Schema.For<IGDBGameFlat>()
            .Index(x => x.Id)
            .Index(x => new
            {
                x.GameType.Id, x.TotalRatingCount, x.TotalRating
            }, idx => { idx.Name = "idx_default_filter"; })
            .Duplicate(x => x.TotalRating, configure: idx =>
            {
                idx.SortOrder = SortOrder.Desc;
            })
            .Duplicate(x => x.TotalRatingCount, configure: idx =>
            {
                idx.SortOrder = SortOrder.Desc;
            })
            .Duplicate(x => x.GameType.Id)
            .Index(x => x.GameModes, configure: idx =>
            {
                idx.Method = IndexMethod.gin;
            })
            .Index(x => x.Genres, configure: idx =>
            {
                idx.Method = IndexMethod.gin;
            })
            .Index(x => x.Platforms, configure: idx =>
            {
                idx.Method = IndexMethod.gin;
            });
    }
}