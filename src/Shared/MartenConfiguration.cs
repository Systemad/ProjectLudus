using Marten;
using Shared.Features;
using Shared.Features.Games;
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

        options.Schema.For<GameEngine>().Index(x => x.Id);
        options.Schema.For<GameMode>().Index(x => x.Id);
        options.Schema.For<Genre>().Index(x => x.Id);
        options.Schema.For<Keyword>().Index(x => x.Id);
        options.Schema.For<PlayerPerspective>().Index(x => x.Id);
        options.Schema.For<Platform>().Index(x => x.Id);
        options.Schema.For<Theme>().Index(x => x.Id);
        options.Schema.For<Company>().Index(x => x.Id).FullTextIndex(x => x.Name);
    }
}