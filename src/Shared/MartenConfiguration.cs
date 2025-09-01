using Marten;
using Shared.Features;
using Shared.Features.Games;
using Weasel.Postgresql;
using Weasel.Postgresql.Tables;

namespace Shared;

public static class MartenSchema
{
    public static void Configure(StoreOptions options)
    {
        options.Schema.For<IGDBGameFlat>().GinIndexJsonData();
        options.Schema.For<IGDBGameFlat>()
            .NgramIndex(x => x.SearchField)
            .UniqueIndex(x => x.Id)
            .Index(x => new
            {
                x.GameType.Id, x.TotalRatingCount, x.TotalRating
            }, idx => { idx.Name = "idx_default_filter"; })
            .Duplicate(x => x.TotalRating, configure: idx => { idx.SortOrder = SortOrder.Desc; })
            .Duplicate(x => x.TotalRatingCount, configure: idx => { idx.SortOrder = SortOrder.Desc; })
            .Duplicate(x => x.GameType.Id)
            .Index(x => x.GameModes)
            .Index(x => x.Genres)
            .Index(x => x.Platforms);

        options.Storage.ExtendedSchemaObjects.Add(new Extension("postgis"));
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