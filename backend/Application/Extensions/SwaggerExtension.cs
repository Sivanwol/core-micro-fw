using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Mvc.ApiExplorer;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;

namespace Application.Extensions; 

public static class SwaggerExtension {

    public static void AddSwaggerExtension(this IServiceCollection services, IConfiguration configuration,
        string applicationTitle, string version = "v1") {
        // the documentation is only available in development or when the environment variable is set to true
        services.AddSwaggerGen(c => {
            c.SwaggerDoc("v1", new() { Title = applicationTitle, Version = version });
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