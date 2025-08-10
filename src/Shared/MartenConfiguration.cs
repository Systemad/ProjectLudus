using Marten;
using Shared.Features;
using Shared.Features.Games;
using Weasel.Postgresql.Tables;

namespace Shared;

public class MartenSchema
{
    public static void Configure(StoreOptions options)
    {
        //options.Schema.For<InsertIGDBGame>().GinIndexJsonData();
        options.Schema.For<InsertIGDBGame>().FullTextIndex(x => x.Name, x => x.AlternativeNames);
        options.Schema.For<InsertIGDBGame>()
            .Duplicate(x => x.Genres)
            .Duplicate(x => x.Platforms)
            .Duplicate(x => x.GameModes)
            .Duplicate(x => x.GameEngines)
            .Duplicate(x => x.Keywords)
            .Duplicate(x => x.Themes)
            .Duplicate(x => x.PlayerPerspectives);

        //options.Schema.For<Platform>().FullTextIndex(x => x.Name, x => x.Slug);
        options.Schema.For<Platform>().Duplicate(x => x.Name);
        //options.Schema.For<Company>().Duplicate(x => x.Id);
        //.Duplicate(x => x.MultiplayerModes);
    }
}