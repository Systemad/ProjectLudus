using JasperFx;
using JasperFx.CodeGeneration;
using Marten;
using Shared;
namespace WebAPI.Configuration;

public static class MartenExtension
{
    public static IServiceCollection RegisterMarten(
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
            .AddMarten(MartenSchema.Configure)
            .ApplyAllDatabaseChangesOnStartup()
            .UseLightweightSessions()
            .UseNpgsqlDataSource();

        services.CritterStackDefaults(x =>
        {
            x.Production.GeneratedCodeMode = TypeLoadMode.Static;
            x.Production.ResourceAutoCreate = AutoCreate.None;
        });
        return services;
    }
}
