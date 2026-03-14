using Microsoft.AspNetCore.OpenApi;
using Microsoft.OpenApi;

namespace CatalogAPI.Extensions;

// https://johnnyreilly.com/dotnet-openapi-and-openapi-ts
/// <summary>
/// Transforms OpenAPI schema for integer types to ensure they are represented
/// with proper type and format, removing unwanted pattern and string type alternatives.
/// This affects integer types like int, long, short, etc.
/// </summary>
public sealed class IntegerSchemaTransformer : IOpenApiSchemaTransformer
{
    public Task TransformAsync(
        OpenApiSchema schema,
        OpenApiSchemaTransformerContext context,
        CancellationToken cancellationToken)
    {
        var type = context.JsonTypeInfo.Type;

        // Handle nullable integers
        var actualType = Nullable.GetUnderlyingType(type) ?? type;

        // Check if this is an integer type
        if (actualType == typeof(int) ||
            actualType == typeof(long) ||
            actualType == typeof(short) ||
            actualType == typeof(byte) ||
            actualType == typeof(sbyte) ||
            actualType == typeof(uint) ||
            actualType == typeof(ulong) ||
            actualType == typeof(ushort))
        {
            // Set type to integer only (not ["integer", "string"])
            schema.Type = JsonSchemaType.Integer;

            // Clear any pattern that might have been added
            schema.Pattern = null;

            // Set appropriate format based on the actual type
            schema.Format = actualType switch
            {
                // based on https://spec.openapis.org/oas/v3.1.1.html#data-types
                Type t when t == typeof(int) => "int32",
                Type t when t == typeof(uint) => "int32",
                Type t when t == typeof(long) => "int64",
                Type t when t == typeof(ulong) => "int64",
                Type t when t == typeof(short) => "int32",
                Type t when t == typeof(ushort) => "int32",
                Type t when t == typeof(byte) => "int32",
                Type t when t == typeof(sbyte) => "int32",
                _ => "int32"
            };

            // Clear any enum values that might have been set
            schema.Enum?.Clear();
        }

        return Task.CompletedTask;
    }
}

/// <summary>
/// Transforms OpenAPI schema for number types to ensure they are represented
/// with proper type and format, removing unwanted pattern and string type alternatives.
/// This affects floating-point types like double, float, and decimal.
/// </summary>
public sealed class NumberSchemaTransformer : IOpenApiSchemaTransformer
{
    public Task TransformAsync(
        OpenApiSchema schema,
        OpenApiSchemaTransformerContext context,
        CancellationToken cancellationToken)
    {
        var type = context.JsonTypeInfo.Type;

        // Handle nullable numbers
        var actualType = Nullable.GetUnderlyingType(type) ?? type;

        // Check if this is an integer type
        if (actualType == typeof(double) ||
            actualType == typeof(decimal) ||
            actualType == typeof(float))
        {
            // Set type to integer only (not ["number", "string"])
            schema.Type = JsonSchemaType.Number;

            // Clear any pattern that might have been added
            schema.Pattern = null;

            // Set appropriate format based on the actual type
            schema.Format = actualType switch
            {
                // based on https://spec.openapis.org/oas/v3.1.1.html#data-types
                Type t when t == typeof(double) => "double",
                Type t when t == typeof(decimal) => "double",
                Type t when t == typeof(float) => "float",
                _ => "double"
            };

            // Clear any enum values that might have been set
            schema.Enum?.Clear();
        }

        return Task.CompletedTask;
    }
}