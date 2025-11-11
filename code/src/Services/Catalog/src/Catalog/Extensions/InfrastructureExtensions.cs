using Catalog.Data;
using FastEndpoints;
using FastEndpoints.Swagger;
using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Npgsql;
using ServiceDefaults;

namespace Catalog.Extensions;

public static class InfrastructureExtensions
{
    public static WebApplicationBuilder AddDatabaseInfrastructure(this WebApplicationBuilder builder)
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
        
        return builder;
    }

    public static WebApplicationBuilder AddCommonInfrastructure(this WebApplicationBuilder builder)
    {
        builder.AddServiceDefaults();
        builder.Logging.AddConsole();
        builder.Services.AddHttpClient();
        builder.Services.AddHttpContextAccessor();
        builder.Services.AddControllers();
        return builder;
    }

}