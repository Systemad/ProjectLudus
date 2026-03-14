using System.Reflection;
using Microsoft.AspNetCore.OpenApi;
using Microsoft.OpenApi;

namespace CatalogAPI.Extensions;

public class RequiredPropertySchemaTransformer : IOpenApiSchemaTransformer
{
    public Task TransformAsync(OpenApiSchema schema, OpenApiSchemaTransformerContext context, CancellationToken cancellationToken)
    {
        if (schema.Properties is null || schema.Properties.Count == 0)
            return Task.CompletedTask;

        var type = context.JsonTypeInfo.Type;
        if (!type.IsClass && !type.IsValueType)
            return Task.CompletedTask;

        // Initialize Required collection if needed
        schema.Required ??= new HashSet<string>();

        // Get the nullability context for the type
        var nullabilityContext = new NullabilityInfoContext();
        var requiredProperties = new HashSet<string>();

        foreach (var property in type.GetProperties(BindingFlags.Public | BindingFlags.Instance))
        {
            // Use JsonTypeInfo to get the effective JSON property name (respects [JsonPropertyName] and naming policy)
            var schemaPropertyName = JsonPropertyNameResolver.GetJsonPropertyName(context.JsonTypeInfo, property);
            if (schemaPropertyName is null)
                continue;

            // Check if property exists in schema
            if (!schema.Properties.ContainsKey(schemaPropertyName))
                continue;

            // Skip properties that are already marked as required
            if (schema.Required.Contains(schemaPropertyName))
                continue;

            // Determine if property should be required
            if (IsPropertyRequired(property, nullabilityContext))
            {
                requiredProperties.Add(schemaPropertyName);
            }
        }

        // Add required properties to schema
        foreach (var propertyName in requiredProperties)
        {
            schema.Required.Add(propertyName);
        }

        return Task.CompletedTask;
    }

    private static bool IsPropertyRequired(PropertyInfo property, NullabilityInfoContext nullabilityContext)
    {
        var propertyType = property.PropertyType;

        // Non-nullable value types are always required (except when wrapped in Nullable<T>)
        if (propertyType.IsValueType)
        {
            // Nullable<T> is optional, plain value types are required
            return Nullable.GetUnderlyingType(propertyType) is null;
        }

        // For reference types, check nullability annotations
        try
        {
            var nullabilityInfo = nullabilityContext.Create(property);
            return nullabilityInfo.WriteState == NullabilityState.NotNull;
        }
        catch
        {
            // If we can't determine nullability, default to not required
            return false;
        }
    }
}