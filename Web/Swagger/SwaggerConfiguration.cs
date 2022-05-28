using System.Reflection;
using Microsoft.Extensions.Options;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;
using Swashbuckle.AspNetCore.SwaggerUI;

namespace Web.Swagger;

public class SwaggerConfiguration : IConfigureOptions<SwaggerGenOptions>, IConfigureOptions<SwaggerUIOptions>
{
    private const string Name = "MosCityHack API";

    public void Configure(SwaggerGenOptions c)
    {
        c.SwaggerDoc("v1", new OpenApiInfo
        {
            Title = Name,
            Description = "Lorem ipsum dolor sit amet (придумать нормальное описание)",
            Version = "v1.0"
        });

        c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
        {
            Name = "Authorization",
            Type = SecuritySchemeType.Http,
            In = ParameterLocation.Header,
            Scheme = "bearer",
            BearerFormat = "JWT",
            Description = "JWT Authorization header using the Bearer scheme."
        });

        c.CustomOperationIds(api => api.TryGetMethodInfo(out var methodInfo) ? methodInfo.Name : null);

        c.UseOneOfForPolymorphism();
        c.UseAllOfForInheritance();
        c.UseInlineDefinitionsForEnums();
        c.UseAllOfToExtendReferenceSchemas();

        // Set the comments path for the Swagger JSON and UI.
        c.IncludeXmlComments(
            Path.Combine(AppContext.BaseDirectory,
                $"{Assembly.GetExecutingAssembly().GetName().Name}.xml"));

        c.OperationFilter<AuthResponsesOperationFilter>();
        c.SchemaFilter<SkipPropertySchemaFilter>();
        c.OperationFilter<SkipPropertySchemaFilter>();
    }

    public void Configure(SwaggerUIOptions c)
    {
        c.DocumentTitle = Name;
        c.SwaggerEndpoint("/swagger/v1/swagger.json", Name);

        c.DisplayOperationId();
        c.ShowExtensions();
        c.InjectStylesheet(
            "https://rawcdn.githack.com/Amoenus/SwaggerDark/5106ba4b198fd5f9fbde7ea4bca248f9ff55ac6a/SwaggerDark.css");
    }
}