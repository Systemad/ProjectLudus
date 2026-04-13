namespace CatalogAPI.Extensions;

internal class RequiredSchemaTransformer : IOpenApiSchemaTransformer
{
    public Task TransformAsync(OpenApiSchema schema, OpenApiSchemaTransformerContext context, CancellationToken cancellationToken)
    {
        if (schema.Properties == null || context.JsonTypeInfo?.Properties == null)
        {
            return Task.CompletedTask;
        }

        foreach (var property in schema.Properties)
        {
            var jsonProperty = context.JsonTypeInfo.Properties.FirstOrDefault(x => x.Name == property.Key);
            if (jsonProperty == null)
            {
                continue;
            }

            if (!jsonProperty.IsGetNullable)
            {
                schema.Required ??= new HashSet<string>();
                schema.Required.Add(property.Key);
            }
        }

        return Task.CompletedTask;
    }
}