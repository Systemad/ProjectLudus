using CatalogAPI.Data;
using Microsoft.EntityFrameworkCore;
using Npgsql;

namespace Catalog.Ingester.Extensions;

public static class ConfigureDatabaseExtensions
{
    public static void AddNpgsql(this WebApplicationBuilder builder, string connectionString)
    {
        builder.AddNpgsqlDbContext<CatalogContext>(
            connectionName: "catalog-db",
            configureDbContextOptions: (optionsBuilder) =>
            {
                optionsBuilder.UseQueryTrackingBehavior(QueryTrackingBehavior.NoTracking);
                optionsBuilder.UseNpgsql(np =>
                {
                    //np.MapEnum<IgdbReference>("type");
                    np.UseNodaTime();
                    np.ConfigureDataSource(x =>
                    {
                        x.EnableParameterLogging();
                        x.UseNodaTime();
                        //x.EnableDynamicJson();
                    });
                });
                //optionsBuilder.UseSnakeCaseNamingConvention();
                //optionsBuilder.UseBulkInsertPostgreSql();
            }
        );

        builder.EnrichNpgsqlDbContext<CatalogContext>();
    }
}