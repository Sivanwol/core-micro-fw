using Application;
using Application.Configs;
using Application.Extensions;
using Application.Utils;
using Domain.Persistence.Context;
using HealthChecks.UI.Client;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using Microsoft.FeatureManagement;
using Serilog;

namespace DevopsTools;

public class Bootstrap {
    public Bootstrap(IConfiguration configuration, IWebHostEnvironment env) {
        Configuration = configuration;
        Environment = env;
        ActiveAzureConfigConnectionString = Configuration["AzureConfigConnectionString"]!;
        ActiveConnectionString = Configuration.GetConnectionString("DomainConnection");
        Log.Information($"Connect to Domain: {ActiveConnectionString}");
        Log.Information($"Connect to Application Config: {ActiveAzureConfigConnectionString}");
    }

    public static IWebHostEnvironment Environment { get; set; }

    public static IConfiguration Configuration { get; set; }
    public static string ActiveAzureConfigConnectionString { get; private set; }
    public static string ActiveConnectionString { get; set; }

    public void ConfigureServices(IServiceCollection services) {
        Log.Information("Start Configure Server");
        services.AddAzureAppConfiguration()
            .AddFeatureManagement();
        services.Configure<DevopsApplicationConfig>(Configuration.GetSection("ApplicationConfig"));
        var applicationConfig = Configuration.GetSection("ApplicationConfig").Get<DevopsApplicationConfig>()!;
        services.AddGenericServiceExtension(Configuration, () => {
            services.AddDbContext<DomainContext>(options => options.UseSqlServer(ActiveConnectionString));
            services.AddTransient<IDomainContext>(provider => provider.GetService<DomainContext>());
        });
        if (Environment.IsDevelopment()  || applicationConfig.EnableSwagger) {
            services.AddSwaggerExtension(applicationConfig, "Devops Tools Docs");
        }

        services.AddHealthChecksUI().AddSqlServerStorage(ActiveConnectionString);
    }

    public void Configure(IApplicationBuilder app, IWebHostEnvironment env) {
        var applicationConfig = Configuration.GetSection("ApplicationConfig").Get<DevopsApplicationConfig>()!;
        if (Environment.IsDevelopment() || applicationConfig.EnableSwagger) {
            app.UseSwaggerExtension(env);
        }


        app.UseGenericServiceExtension(env, () => {
            if (!Environment.IsDevelopment()) {
                app.UseHsts();
            }

            app.UseHealthChecks("/healthz", new HealthCheckOptions {
                Predicate = _ => true,
                ResponseWriter = UIResponseWriter.WriteHealthCheckUIResponse,
                ResultStatusCodes = {
                    [HealthStatus.Healthy] = StatusCodes.Status200OK,
                    [HealthStatus.Degraded] = StatusCodes.Status500InternalServerError,
                    [HealthStatus.Unhealthy] = StatusCodes.Status503ServiceUnavailable,
                }
            }).UseHealthChecksUI(setup => {
                setup.ApiPath = "/healthcheck";
                setup.UIPath = "/healthcheck-ui";
            });
        });
        app.UseEndpoints(endpoints => { endpoints.MapControllers(); });
    }
}