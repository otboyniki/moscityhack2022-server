using System.ComponentModel.DataAnnotations.Schema;
using System.Reflection;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace Web.Swagger;

public class SkipPropertySchemaFilter : ISchemaFilter, IOperationFilter
{
    public void Apply(OpenApiSchema schema, SchemaFilterContext context)
    {
        if (schema?.Properties == null)
        {
            return;
        }

        foreach (var skipProperty in context.Type
                                            .GetProperties()
                                            .Where(IsSkippedProperty))
        {
            var propertyToSkip = schema.Properties.Keys.SingleOrDefault(
                x => string.Equals(x, skipProperty.Name, StringComparison.OrdinalIgnoreCase));

            if (propertyToSkip != null)
            {
                schema.Properties.Remove(propertyToSkip);
            }
        }
    }

    public void Apply(OpenApiOperation operation, OperationFilterContext context)
    {
        var ignoredProperties = context.MethodInfo
                                       .GetParameters()
                                       .SelectMany(p =>
                                           p.ParameterType
                                            .GetProperties()
                                            .Where(IsSkippedProperty))
                                       .ToList();

        operation.Parameters = operation.Parameters
                                        .Where(p =>
                                            p.In == ParameterLocation.Path ||
                                            ignoredProperties.All(x =>
                                                !p.Name.Equals(x.Name, StringComparison.InvariantCulture)))
                                        .ToList();
    }

    private static bool IsSkippedProperty(PropertyInfo property) =>
        property.GetCustomAttribute<NotMappedAttribute>() != null;
}