using PlayAPI.Features.Typesense.GetKey;

namespace PlayAPI.Features.Typesense;

public static class TypesenseEndpointMappingExtensions
{
    public static IEndpointRouteBuilder MapTypesenseEndpointsGroup(this IEndpointRouteBuilder app)
    {
        app.MapGetKeyEndpoints();
        return app;
    }
}
