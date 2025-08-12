using JasperFx;
using JasperFx.CodeGeneration;
using Marten;
using Shared;
using Shared.Features.Games;

namespace WebAPI.Configuration;

public static class MartenConfiguration
{
    public static long ToUnixTimestamp(int year, int month, int day)
    {
        var dt = new DateTime(year, month, day, 0, 0, 0, DateTimeKind.Utc);
        return new DateTimeOffset(dt).ToUnixTimeSeconds();
    }
    
    public static IServiceCollection AddMartenDatabases(
        this IServiceCollection services,
        IHostEnvironment env,
        IConfiguration config
    )
    {
        var connectionString = config["IGDB_DB"];

        if (connectionString == null)
        {
            throw new ArgumentException("ConnectionString cannot be null!");
        }
        
        services.AddNpgsqlDataSource(connectionString);
        services.AddLogging();
        
        services
            .AddMarten(options =>
            {
                options.UseSystemTextJsonForSerialization();
                options.AutoCreateSchemaObjects = AutoCreate.All;
                
                MartenSchema.Configure(options);
            })
            .ApplyAllDatabaseChangesOnStartup()
            .UseLightweightSessions()
            .UseNpgsqlDataSource();

        /*
        services.CritterStackDefaults(x =>
        {
            x.Production.GeneratedCodeMode = TypeLoadMode.Static;
            x.Production.ResourceAutoCreate = AutoCreate.None;
        });
        */
        return services;
    }
}
