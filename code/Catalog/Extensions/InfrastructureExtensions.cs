using Catalog.Data;
using FastEndpoints;
using FastEndpoints.Swagger;
using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Hosting;
using Npgsql;

namespace Catalog.Extensions;

public static class InfrastructureExtensions
{
    public static WebApplicationBuilder AddInfrastructure(this WebApplicationBuilder builder)
    {
        builder.AddServiceDefaults();
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
        
        builder
            .Services.AddFastEndpoints()
            .SwaggerDocument(options =>
            {
                options.DocumentSettings = s =>
                {
                    s.Title = "Catalog API";
                    s.Version = "v1";
                };
                options.ShortSchemaNames = true;
                options.DocumentSettings = s =>
                {
                    s.MarkNonNullablePropsAsRequired();
                };
            });
        
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
}