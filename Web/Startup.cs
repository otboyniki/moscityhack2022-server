using System.Text.Json;
using System.Text.Json.Serialization;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.HttpLogging;
using Microsoft.EntityFrameworkCore;
using Web.Data;
using Web.Extensions;
using Web.Services;
using Web.Swagger;

namespace Web;

public class Startup
{
    private static void ConfigureJsonSerializer(JsonSerializerOptions options)
    {
        options.PropertyNamingPolicy = JsonNamingPolicy.CamelCase;
        options.Converters.Add(new JsonStringEnumConverter(options.PropertyNamingPolicy));
    }

    public void ConfigureServices(IServiceCollection services)
    {
        services.AddControllers()
                .AddJsonOptions(options => ConfigureJsonSerializer(options.JsonSerializerOptions));

        services.AddDbContext<DataContext>((ctx, options) =>
                                               options.UseNpgsql(ctx.GetRequiredService<IConfiguration>()
                                                                    .GetConnectionString("DefaultConnection"),
                                                                 b => b.UseNetTopologySuite())
                                                      .UseLoggerFactory(ctx.GetRequiredService<ILoggerFactory>())
                                                      .EnableDetailedErrors()
                                                      .EnableSensitiveDataLogging());

        services.AddSwaggerGen();
        services.ConfigureOptions<SwaggerConfiguration>();

        services.AddHttpLogging(options =>
        {
            options.LoggingFields = HttpLoggingFields.RequestHeaders | HttpLoggingFields.RequestBody;
        });

        services.AddCors(options => options.AddPolicy("CorsPolicy", builder =>
                                                          builder.AllowAnyMethod()
                                                                 .AllowAnyHeader()
                                                                 .SetIsOriginAllowed(_ => true)
                                                                 .AllowCredentials()));

        services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
                .AddCookie(options => options.Events = new CookieAuthenticationEvents
                {
                    OnRedirectToLogin = context =>
                    {
                        context.Response.StatusCode = 401;
                        return Task.CompletedTask;
                    },
                    OnRedirectToAccessDenied = context =>
                    {
                        context.Response.StatusCode = 403;
                        return Task.CompletedTask;
                    },
                });
        services.AddAuthorization();

        services.AddScoped<FileService>();
    }

    public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
    {
        app.UseApiExceptionHandler();

        app.UseSwagger()
           .UseSwaggerUI();

        app.UseRouting();
        app.UseCors("CorsPolicy");

        app.UseAuthentication();
        app.UseAuthorization();
        app.UseHttpLogging();

        app.UseStaticFiles();
        app.UseEndpoints(endpoints => endpoints.MapControllers());

        InitializeDatabase(app.ApplicationServices);
    }

    private void InitializeDatabase(IServiceProvider serviceProvider)
    {
        Seeder.Migrate(serviceProvider);
    }
}