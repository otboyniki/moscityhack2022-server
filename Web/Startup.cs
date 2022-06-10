using System.Text.Json;
using System.Text.Json.Serialization;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.HttpLogging;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Web.Data;
using Web.Data.Entities;
using Web.Extensions;

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
                                 .GetConnectionString("DefaultConnection"))
                   .UseLoggerFactory(ctx.GetRequiredService<ILoggerFactory>())
                   .EnableDetailedErrors()
                   .EnableSensitiveDataLogging());

        services.AddIdentity<User, Role>(o =>
                {
                    o.Password.RequireDigit = false;
                    o.Password.RequireLowercase = false;
                    o.Password.RequireUppercase = false;
                    o.Password.RequireNonAlphanumeric = false;
                    o.Password.RequiredLength = 4;
                })
                .AddEntityFrameworkStores<DataContext>()
                .AddDefaultTokenProviders();

        services.AddSwaggerGen();
        services.AddHttpLogging(options =>
        {
            options.LoggingFields = HttpLoggingFields.RequestHeaders | HttpLoggingFields.RequestBody;
        });

        services.AddCors(options => options.AddPolicy("CorsPolicy", builder =>
            builder.AllowAnyOrigin()
                   .AllowAnyMethod()
                   .AllowAnyHeader()));

        services.AddAuthentication("AspNetCore.Identity.Application")
                .Services.ConfigureApplicationCookie(o =>
                {
                    o.Cookie.Name = ".AspNetCore.Identity.Application";
                    o.Events = new CookieAuthenticationEvents
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
                        }
                    };
                });
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
        Seeder.Seed(serviceProvider).Wait();
    }
}