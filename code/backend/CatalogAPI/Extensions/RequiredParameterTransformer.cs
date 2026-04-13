using Microsoft.AspNetCore.Mvc.ApiExplorer;
using Microsoft.AspNetCore.Mvc.Controllers;
using System.Reflection;

namespace CatalogAPI.Extensions;

    internal class RequiredParameterTransformer : IOpenApiOperationTransformer
    {
        private static readonly NullabilityInfoContext NullabilityContext = new();

        public Task TransformAsync(OpenApiOperation operation, OpenApiOperationTransformerContext context, CancellationToken cancellationToken)
        {
            if (operation.Parameters == null || operation.Parameters.Count == 0)
            {
                return Task.CompletedTask;
            }

            foreach (var parameter in operation.Parameters)
            {
                if (parameter.Required)
                {
                    continue;
                }

                var apiParameter = context.Description.ParameterDescriptions.FirstOrDefault(x =>
                    string.Equals(x.Name, parameter.Name, StringComparison.OrdinalIgnoreCase));

                if (apiParameter is null)
                {
                    continue;
                }

                if (IsNonNullable(apiParameter))
                {
                    if (parameter is OpenApiParameter openApiParameter)
                    {
                        openApiParameter.Required = true;
                    }
                }
            }

            return Task.CompletedTask;
        }

        private static bool IsNonNullable(ApiParameterDescription parameterDescription)
        {
            var type = parameterDescription.Type;
            if (type is null)
            {
                return false;
            }

            if (type.IsValueType)
            {
                return Nullable.GetUnderlyingType(type) is null;
            }

            if (parameterDescription.ParameterDescriptor is ControllerParameterDescriptor controllerParameter)
            {
                var nullabilityInfo = NullabilityContext.Create(controllerParameter.ParameterInfo);
                return nullabilityInfo.ReadState == NullabilityState.NotNull;
            }

            return false;
        }
    }