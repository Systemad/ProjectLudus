using Backend.API.Features.UserLibrary;

namespace Backend.API.Extensions;

public static class UserLibraryFeatureExtensions
{
    public static WebApplicationBuilder AddUserLibraryFeature(this WebApplicationBuilder builder) =>
        builder;

    public static WebApplication MapUserLibraryFeature(this WebApplication app)
    {
        app.MapUserLibraryEndpoints();

        return app;
    }
}
