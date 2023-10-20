using Application.Configs;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
namespace Application.Extensions;

public static class ApiVersionExtension {
    public static void AddApiVersionExtension(this IServiceCollection services, BackendApplicationConfig configuration) {
        services.AddVersionedApiExplorer(o => {
            o.GroupNameFormat = "'v'VVV";
            o.SubstituteApiVersionInUrl = true;
        });
        services.AddApiVersioning(config => {
            // Specify the default API Version as 1.0
            config.DefaultApiVersion = new ApiVersion(configuration.APIMajorVersion, configuration.APIMinorVersion);
            // If the client hasn't specified the API version in the request, use the default API version number 
            config.AssumeDefaultVersionWhenUnspecified = true;
            // Advertise the API versions supported for the particular endpoint
            config.ReportApiVersions = true;
        });

    }
    // public static void UseApiVersionExtension(this IApplicationBuilder app, ApplicationConfig configuration) { }
}