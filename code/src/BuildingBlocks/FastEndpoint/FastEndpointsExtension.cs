using FastEndpoints;
using FastEndpoints.Swagger;
using Microsoft.AspNetCore.Builder;

namespace BuildingBlocks.FastEndpoint;

public static class FastEndpointsExtension
{
    public static WebApplicationBuilder AddFastEndpoints<T>(
        this WebApplicationBuilder builder,
        string title
    )
    {
        builder
            .Services.AddFastEndpoints(o => o.Assemblies = new[] { typeof(T).Assembly })
            .SwaggerDocument(options =>
            {
                options.DocumentSettings = s =>
                {
                    s.Title = title;
                    s.Version = "v1";
                };
                options.ShortSchemaNames = true;
                options.DocumentSettings = s =>
                {
                    s.MarkNonNullablePropsAsRequired();
                };
            });
        return builder;
    }

    public static IApplicationBuilder UseFastEndpointsExt(this WebApplication app)
    {
        app.UseFastEndpoints(x =>
            {
                x.Errors.UseProblemDetails();
                //x.Endpoints.ShortNames = true;
            })
            .UseSwaggerGen(c => { });
        return app;
    }
}
