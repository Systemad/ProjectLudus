using Catalog.Data;
using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Npgsql;

namespace Catalog.Extensions;

public static class InfrastructureExtensions
{
    public static WebApplicationBuilder AddDatabaseInfrastructure(
        this WebApplicationBuilder builder
    )
    {
        builder.AddNpgsqlDbContext<CatalogDbContext>(
            connectionName: ConnectionStrings.CatalogPrimary,
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
                        x.EnableDynamicJson();
                    });
                });
                optionsBuilder.UseSnakeCaseNamingConvention();
                //optionsBuilder.UseBulkInsertPostgreSql();
            }
        );

        builder.EnrichNpgsqlDbContext<CatalogDbContext>();
        return builder;
    }
    
    public static HostApplicationBuilder AddDatabaseInfrastructure(
        this HostApplicationBuilder builder
    )
    {
        builder.AddNpgsqlDbContext<CatalogDbContext>(
            connectionName: ConnectionStrings.CatalogPrimary,
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
                        x.EnableDynamicJson();
                    });
                });
                optionsBuilder.UseSnakeCaseNamingConvention();
                //optionsBuilder.UseBulkInsertPostgreSql();
            }
        );

        builder.EnrichNpgsqlDbContext<CatalogDbContext>();
        return builder;
    }

    public static HostApplicationBuilder AddNpgsqlInfrastructre(this HostApplicationBuilder builder)
    {
        // ADD STATIC CLASS, TO AVOID HARDCODING STRINGS
        builder.Services.AddNpgsqlDataSource(
            ConnectionStrings.CatalogPrimary,
            o =>
            {
                o.UseNodaTime();
            }
        );
        return builder;
    }

    public static WebApplicationBuilder AddCommonInfrastructure(this WebApplicationBuilder builder)
    {
        builder.Logging.AddConsole();
        builder.Services.AddHttpClient();
        builder.Services.AddHttpContextAccessor();
        builder.Services.AddControllers();
        return builder;
    }
}

/*
builder.Services.AddScoped<ApiClient>();
builder
    .Services.AddHttpClient<ApiClient>(httpClient =>
    {
        httpClient.BaseAddress = new Uri("https://api.igdb.com/v4/");
        httpClient.DefaultRequestHeaders.Add("Accept", "application/json");
    })
    .AddHttpMessageHandler<TwitchAuthenticationHandler>();
*/
