using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Scalar.AspNetCore;

namespace BuildingBlocks.OpenApi;

public static class Extensions
{
    public static IApplicationBuilder UseAspnetOpenApi(this WebApplication app)
    {
        app.UseOpenApi(c => c.Path = "/openapi/v1.json");
        app.MapScalarApiReference();
        app.Map("/scalar", () => Results.Redirect("/scalar/v1"));

        return app;
    }
}
