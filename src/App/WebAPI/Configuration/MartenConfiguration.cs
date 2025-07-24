using JasperFx;
using Marten;
using Shared.Features.Games;
using Weasel.Postgresql.Tables;
using WebAPI.Features.Common;

namespace WebAPI.Configuration;

public static class MartenConfiguration
{
    public static IServiceCollection AddMartenDatabases(
        this IServiceCollection services,
        IHostEnvironment env,
        IConfiguration config
    )
    {
        //string key = "host=localhost:5432;database=gamingdb;password=Compaq2009;username=dan1";
        string key =
            "host=localhost:5432;database=gamingdb;CommandTimeout=500;password=Compaq2009;username=dan1";
        services.AddNpgsqlDataSource(key);

        services
            .AddMarten(options =>
            {
                options.UseSystemTextJsonForSerialization();

                if (env.IsDevelopment())
                {
                    options.AutoCreateSchemaObjects = AutoCreate.All;
                }
                else
                {
                    options.AutoCreateSchemaObjects = AutoCreate.CreateOrUpdate;
                }

                options.Schema.For<IGDBGame>().FullTextIndex(x => x.Name, x => x.AlternativeNames);

                options
                    .Schema.For<IGDBGame>()
                    .Duplicate(x => x.TotalRating)
                    .Duplicate(x => x.Rating)
                    .Duplicate(x => x.RatingCount)
                    .Duplicate(x => x.CreatedAt)
                    .Duplicate(x => x.FirstReleaseDate)
                    .Duplicate(x => x.GameType.Id);
                /*
                .Duplicate(x => x.Genres.Select(genre => genre.Id).ToArray())
                .Duplicate(x => x.Platforms.Select(platform => platform.Id).ToArray())
                .Duplicate(x => x.Themes.Select(theme => theme.Id).ToArray())
                .Duplicate(x => x.GameModes.Select(gameMode => gameMode.Id).ToArray())
                .Duplicate(x => x.GameEngines.Select(gameEngine => gameEngine.Id).ToArray())
                .Duplicate(x => x.PlayerPerspectives.Select(pps => pps.Id).ToArray())
                */
                ;

                //.Duplicate(x => x.Slug)
                //.Duplicate(x => x.Name)
                /*
                options.Schema.For<Game>().FullTextIndex(x => x.Name);
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
                */
            })
            .UseLightweightSessions()
            .UseNpgsqlDataSource()
            .ApplyAllDatabaseChangesOnStartup();

        return services;
    }
}
