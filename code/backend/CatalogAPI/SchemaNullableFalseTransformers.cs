using System.Reflection;
using Microsoft.AspNetCore.OpenApi;
using Microsoft.OpenApi;

namespace CatalogAPI;
/*
public sealed class RequiredSchemaTransformer : IOpenApiSchemaTransformer
{
    private readonly NullabilityInfoContext _nullability = new();

    public Task TransformAsync(
        OpenApiSchema schema,
        OpenApiSchemaTransformerContext context,
        CancellationToken cancellationToken)
    {
        if (schema.Properties is null)
            return Task.CompletedTask;

        schema.Required ??= new HashSet<string>(StringComparer.Ordinal);

        foreach (var jsonProperty in context.JsonTypeInfo.Properties)
        {
            if (jsonProperty.AttributeProvider is not PropertyInfo propertyInfo)
                continue;

            var propertyName = jsonProperty.Name;

            if (!schema.Properties.TryGetValue(propertyName, out var propertySchema))
                continue;

            var nullability = _nullability.Create(propertyInfo);

            bool isNullable =
                nullability.WriteState == NullabilityState.Nullable ||
                nullability.ReadState == NullabilityState.Nullable;

            // Remove "null" from schema types
            if (propertySchema.Types is not null)
            {
                propertySchema.Types.Remove("null");
            }

            // Mark required only if property isn't nullable
            if (!isNullable)
            {
                schema.Required.Add(propertyName);
            }
        }

        return Task.CompletedTask;
    }
}
*/
/*
public sealed class RequiredSchemaTransformer : IOpenApiSchemaTransformer
{
    private readonly NullabilityInfoContext _nullabilityInfoContext = new();

    public Task TransformAsync(OpenApiSchema schema, OpenApiSchemaTransformerContext context,
        CancellationToken cancellationToken)
    {
        if (schema.Properties == null) return Task.CompletedTask;

        schema.Required ??= new HashSet<string>(StringComparer.Ordinal);

        foreach (var jsonPropertyInfo in context.JsonTypeInfo.Properties)
        {
            if (jsonPropertyInfo.AttributeProvider is not PropertyInfo propertyInfo)
                continue;

            var nullability = _nullabilityInfoContext.Create(propertyInfo);

            if (nullability.WriteState == NullabilityState.NotNull ||
                (nullability.WriteState == NullabilityState.Unknown && propertyInfo.PropertyType.IsAbstract))
            {
                schema.Required.Add(jsonPropertyInfo.Name);
            }
            else if (nullability is { WriteState: NullabilityState.Unknown, ReadState: NullabilityState.NotNull })
            {
                if (schema.Properties.TryGetValue(jsonPropertyInfo.Name, out var value) &&
                    value is OpenApiSchema schemaValue)
                {
                    schemaValue.ReadOnly = true;
                    if (schemaValue.Type != null) schemaValue.Type = schemaValue.Type.Value & ~JsonSchemaType.Null;
                }

                schema.Required.Add(jsonPropertyInfo.Name);
            }
        }

        return Task.CompletedTask;
    }
}
*/
/*
public class SchemaNullableFalseTransformers : IOpenApiSchemaTransformer
{
    public Task TransformAsync(
        OpenApiSchema schema,
        OpenApiSchemaTransformerContext context,
        CancellationToken cancellationToken
    )
    {
        if (schema.Properties is not null)
        {
            foreach (var property in schema.Properties)
            {
                if (schema.Required?.Contains(property.Key) != true)
                {
                    property.Value.Nullable = false;
                }
            }
        }

        return Task.CompletedTask;
    }
}
*/