
using System.Reflection;
using System.Text;
using Application.Extensions;
using Domain.Context;
using HealthChecks.UI.Client;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using Processor;
using Processor.Consumers.IndexUser;
using Processor.Handlers.User.Create;
using Processor.Handlers.User.List;
using Serilog;

namespace FrontApi; 

public class Bootstrap {
    
    public Bootstrap(IConfiguration configuration, IWebHostEnvironment env) {
        Configuration = configuration;
        Environment = env;

        Domain = $"https://{Configuration["Auth0:Domain"]}/";
    }

    private static string Domain;
    public static IWebHostEnvironment Environment { get; set; }

    public static IConfiguration Configuration { get; set; }
    
    public void ConfigureServices(IServiceCollection services) {
        var useLocalRQ = Boolean.Parse(Configuration["ENABLE_SWAGGER"] ?? "false");
        Log.Information("Start Configure Server");
        services.AddGenericServiceExtension(Configuration, Domain, () => {
            services.AddDbContext<DomainContext>(options => options.UseNpgsql("Name=DbConnection"));
            services.AddTransient<IDomainContext>(provider => provider.GetService<DomainContext>());
        });
        if (Environment.IsDevelopment() || useLocalRQ) {
            services.AddSwaggerExtension(Configuration, "Front Api Docs", "V1");
        }
        // services.AddAutoMapper(typeof(DomainProcessorProfile));
        services.AddMediatR(configuration => {
            configuration.RegisterServicesFromAssemblies(Assembly.GetExecutingAssembly());
            configuration.RegisterServicesFromAssemblyContaining(typeof(ListUsersRequest));
            configuration.RegisterServicesFromAssemblyContaining(typeof(CreateUserRequest));
        });
        services.AddElasticsearch(Configuration);
        services.AddMassTransitExtension(Configuration, bus => {
            bus.AddConsumer<IndexUserConsumerHandler>();
        });
        services.AddHealthChecks();
        services.AddHealthChecksUI();
        services.AddHealthChecksUI().AddPostgreSqlStorage(Configuration.GetConnectionString("DbConnection"));
    }

    public void Configure(IApplicationBuilder app, IWebHostEnvironment env) {
        var useLocalRQ = Boolean.Parse(Configuration["ENABLE_SWAGGER"] ?? "false");
        if (Environment.IsDevelopment() || useLocalRQ) {
            app.UseSwaggerExtension(env);
        }
        app.UseGenericServiceExtension(env, () => {
            
            app.UseHealthChecks("/healthz", new HealthCheckOptions
            {
                Predicate = _ => true,
                ResponseWriter = UIResponseWriter.WriteHealthCheckUIResponse,
                ResultStatusCodes =
                {
                    [HealthStatus.Healthy] = StatusCodes.Status200OK,
                    [HealthStatus.Degraded] = StatusCodes.Status500InternalServerError,
                    [HealthStatus.Unhealthy] = StatusCodes.Status503ServiceUnavailable,
                }
            }).UseHealthChecksUI(setup =>
            {
                setup.ApiPath = "/healthcheck";
                setup.UIPath = "/healthcheck-ui";
            });
        });
        app.UseEndpoints(endpoints => {
            endpoints.MapControllers();
        });
    }

}