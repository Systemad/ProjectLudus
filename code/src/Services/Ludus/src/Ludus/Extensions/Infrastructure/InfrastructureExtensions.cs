using Ludus.Data;
using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Npgsql;
using ServiceDefaults;

namespace Ludus.Extensions.Infrastructure;

public static class InfrastructureExtensions
{
    public static WebApplicationBuilder AddDatabaseInfrastructure(this WebApplicationBuilder builder)
    {
        builder.AddServiceDefaults();
        builder.AddNpgsqlDbContext<LudusContext>(
            connectionName: "user-db",
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

        builder.EnrichNpgsqlDbContext<LudusContext>();
        return builder;
    }

    public static WebApplicationBuilder AddCommonInfrastructure(this WebApplicationBuilder builder)
    {
        builder.Logging.AddConsole();
        builder.Services.AddMemoryCache();
        builder.Services.AddHttpClient();
        builder.Services.AddHttpContextAccessor();
        builder.Services.AddControllers();
        return builder;
    }

}