using System.Linq.Expressions;
using Marten;
using Shared.Features.Games;
using Weasel.Core;
using Weasel.Postgresql.Tables;
using WebAPI.Features;
using WebAPI.Features.Common;
using WebAPI.Features.Common.Games.Models;
using WebAPI.Features.Common.Lists;
using WebAPI.Features.Common.Users.Models;

namespace WebAPI.Configuration;

public static class MartenConfiguration
{
    public static IServiceCollection AddMartenDatabases(
        this IServiceCollection services,
        IHostEnvironment env,
        IConfiguration config
    )
    {
        services
            .AddMartenStore<IGameStore>(options =>
            {
                options.Connection(
                    "host=localhost:5432;database=gamingdb;password=Compaq2009;username=dan1"
                );
                options.UseSystemTextJsonForSerialization();

                options.Schema.For<Game>().FullTextIndex(x => x.Name).Identity(x => x.Id);
                options.Schema.For<Game>().Index(x => x.GameType.Id);

                options
                    .Schema.For<Game>()
                    .Index(
                        x => x.Rating,
                        x =>
                        {
                            x.SortOrder = SortOrder.Desc;
                        }
                    );

                if (env.IsDevelopment())
                {
                    options.AutoCreateSchemaObjects = AutoCreate.All;
                }
            })
            .ApplyAllDatabaseChangesOnStartup();

        return services;
    }
}
