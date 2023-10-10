using Application.Extensions;
using Application.Utils;
using Domain.Context;
using HealthChecks.UI.Client;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using Serilog;

namespace DevopsTools;

public class Bootstrap {
    public Bootstrap(IConfiguration configuration, IWebHostEnvironment env) {
        Configuration = configuration;
        Environment = env;
        var useDockerConnectionConfigValue = Configuration["ExtractDockerConnection"] ?? "false";
        var useDockerConnection = Boolean.Parse(useDockerConnectionConfigValue);
        ActiveConnectionString =
            Configuration.GetConnectionString(useDockerConnection ? "DockerConnection" : "DbConnection");
        Log.Information($"Connect to Db Domain: {ActiveConnectionString}");
    }

    public static IWebHostEnvironment Environment { get; set; }

    public static IConfiguration Configuration { get; set; }
    public static string ActiveConnectionString { get; private set; }

    public void ConfigureServices(IServiceCollection services) {
        var useLocalRQ = Boolean.Parse(Configuration["ENABLE_SWAGGER"] ?? "false");
        Log.Information("Start Configure Server");
        services.AddGenericServiceExtension(Configuration, () => {
            services.AddDbContext<DomainContext>(options => options.UseSqlServer(ActiveConnectionString));
            services.AddTransient<IDomainContext>(provider => provider.GetService<DomainContext>());
        });
        if (Environment.IsDevelopment() || useLocalRQ) {
            services.AddSwaggerExtension(Configuration, "Devops Tools Docs", "V1");
        }

        services.AddHealthChecksUI().AddSqlServerStorage(ActiveConnectionString);
    }

    public void Configure(IApplicationBuilder app, IWebHostEnvironment env) {
        var useLocalRQ = Boolean.Parse(Configuration["ENABLE_SWAGGER"] ?? "false");
        if (Environment.IsDevelopment() || useLocalRQ) {
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