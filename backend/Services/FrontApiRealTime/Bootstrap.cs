using System.Reflection;
using Application.Configs;
using Application.Extensions;
using Application.Utils;
using Domain.Persistence.Context;
using Microsoft.EntityFrameworkCore;
using Microsoft.FeatureManagement;
using Processor.Consumers.IndexUser;
using Processor.Services.User.Create;
using Processor.Services.User.List;
using Serilog;
namespace FrontApiRealTime;

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
        // Load configuration from Azure App Configuration
        services.AddAzureAppConfiguration()
            .AddFeatureManagement();
        services.Configure<BackendRealtimeApplicationConfig>(Configuration.GetSection("ApplicationConfig"));
        var applicationConfig = Configuration.GetSection("ApplicationConfig").Get<BackendRealtimeApplicationConfig>()!;
        services.AddGenericServiceExtension(Configuration, () => {
            services.AddDbContext<DomainContext>(options => options.UseSqlServer(ActiveConnectionString));
            services.AddTransient<IDomainContext>(provider => provider.GetService<DomainContext>()!);
        });
        if (Environment.IsDevelopment() || applicationConfig.EnableSwagger) {
            services.AddSwaggerExtension(applicationConfig, "Front Api Realtime Docs");
        }

        services.AddMediatR(configuration => {
            configuration.RegisterServicesFromAssemblies(Assembly.GetExecutingAssembly());
            configuration.RegisterServicesFromAssemblyContaining(typeof(ListUsersRequest));
            configuration.RegisterServicesFromAssemblyContaining(typeof(CreateUserRequest));
        });
        // services.AddElasticsearch(Configuration);
        services.AddMassTransitExtension(applicationConfig, bus => {
            bus.AddConsumer<IndexUserConsumerHandler>();
        });
        services.AddHealthChecks().AddCheck<GeneralHealthCheck>("front_api_real_time_service");
    }

    public void Configure(IApplicationBuilder app, IWebHostEnvironment env) {
        var applicationConfig = Configuration.GetSection("ApplicationConfig").Get<BackendRealtimeApplicationConfig>()!;
        if (Environment.IsDevelopment() || applicationConfig.EnableSwagger) {
            app.UseSwaggerExtension(env);
        }

        app.UseGenericServiceExtension(env, () => {
            if (!Environment.IsDevelopment()) {
                app.UseHsts();
            }
        });
        app.UseEndpoints(endpoints => { endpoints.MapControllers(); });
    }
}