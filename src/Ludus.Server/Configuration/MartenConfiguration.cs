using Ludus.Server.Features;
using Ludus.Server.Features.Lists;
using Ludus.Server.Features.User.Models;
using Ludus.Shared.Features.Games;
using Marten;
using Marten.Services;
using Weasel.Core;

namespace Ludus.Server.Configuration;

public static class MartenConfiguration
{
    public static IServiceCollection AddMartenDatabases(
        this IServiceCollection services,
        IHostEnvironment env,
        IConfiguration config
    )
    {
        services
            .AddMarten(options =>
            {
                options.Connection(
                    "host=localhost:5432;database=userdb;password=Compaq2009;username=dan1"
                );
                //options.OpenTelemetry.TrackConnections = TrackLevel.Normal;
                options.UseSystemTextJsonForSerialization();
                if (env.IsDevelopment())
                {
                    options.AutoCreateSchemaObjects = AutoCreate.All;
                }
                options.Schema.For<User>().Identity(x => x.Id);
                options.Schema.For<UserImage>().Identity(x => x.Id);
                options.Schema.For<GameCollection>().Identity(x => x.Id);
                options.Schema.For<UserGameList>().Identity(x => x.Id);
            })
            //.InitializeWith(new InitialData())
            .ApplyAllDatabaseChangesOnStartup();

        services
            .AddMartenStore<IGameStore>(options =>
            {
                options.Connection(
                    "host=localhost:5432;database=gamingdb;password=Compaq2009;username=dan1"
                );
                options.UseSystemTextJsonForSerialization();

                options.Schema.For<Game>().FullTextIndex(x => x.Name).Identity(x => x.Id);

                if (env.IsDevelopment())
                {
                    options.AutoCreateSchemaObjects = AutoCreate.All;
                }
            })
            .ApplyAllDatabaseChangesOnStartup();

        return services;
    }
}
