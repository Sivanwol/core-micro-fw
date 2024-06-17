using Application.Configs;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.OpenApi.Models;
namespace Application.Extensions;

public static class SwaggerExtension {

    public static void AddSwaggerExtension(this IServiceCollection services, DevopsApplicationConfig configuration, string applicationTitle) {
        AddSwaggerGenSupport(services, $"v{configuration.APIMajorVersion}", applicationTitle);
    }
    public static void AddSwaggerExtension(this IServiceCollection services, BackendApplicationConfig configuration, string applicationTitle) {
        AddSwaggerGenSupport(services, $"v{configuration.APIMajorVersion}", applicationTitle);
    }
    public static void AddSwaggerExtension(this IServiceCollection services, BackendRealtimeApplicationConfig configuration, string applicationTitle) {
        AddSwaggerGenSupport(services, $"v{configuration.APIMajorVersion}", applicationTitle);
    }
    private static void AddSwaggerGenSupport(this IServiceCollection services, string version, string applicationTitle) {
        services.AddSwaggerGen(c => {
            c.IgnoreObsoleteActions();
            c.EnableAnnotations();
            c.SwaggerDoc(version, new OpenApiInfo {
                Title = applicationTitle,
                Version = version,
                Description = "This is the API documentation for the application",
                Contact = new OpenApiContact {
                    Name = "Sivan Wolberg",
                    Email = "sivan@wolberg.pro",
                    Url = new Uri("https://www.linkedin.com/in/swolberg/")
                }
            });
            c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme {
                In = ParameterLocation.Header,
                Description = "Please enter a valid token",
                Name = "Authorization",
                Type = SecuritySchemeType.Http,
                BearerFormat = "JWT",
                Scheme = "Bearer"
            });
            c.AddSecurityRequirement(new OpenApiSecurityRequirement {
                {
                    new OpenApiSecurityScheme {
                        Reference = new OpenApiReference {
                            Type = ReferenceType.SecurityScheme,
                            Id = "Bearer"
                        }
                    },
                    new string[] { }
                }
            });
        });
        services.AddSwaggerGenNewtonsoftSupport();
        services.AddControllers();
    }

    public static void UseSwaggerExtension(this IApplicationBuilder app, IWebHostEnvironment env) {
        app.UseSwagger();
        app.UseSwaggerUI(c => {
            c.SwaggerEndpoint("/swagger/v1/swagger.json", "API Documentation");
        });
    }
}