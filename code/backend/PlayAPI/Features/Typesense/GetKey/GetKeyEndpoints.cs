using Microsoft.AspNetCore.Http.HttpResults;

namespace PlayAPI.Features.Typesense.GetKey;

public static class GetKeyEndpoints
{
    public sealed record TypesenseKeyResponse(string Key, string Index);

    public static IEndpointRouteBuilder MapGetKeyEndpoints(this IEndpointRouteBuilder routeBuilder)
    {
        var group = routeBuilder.MapGroup("/api/typesense");

        group.MapGet(
            "/key",
            async Task<Ok<TypesenseKeyResponse>> (TypesenseKeyService keyService) =>
            {
                var key = await keyService.GetValidKeyAsync();
                return TypedResults.Ok(
                    new TypesenseKeyResponse(key, TypesenseKeyService.SearchCollection)
                );
            }
        );

        return group;
    }
}
