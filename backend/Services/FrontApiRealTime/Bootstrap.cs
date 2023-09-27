using System.Reflection;
using Application.Extensions;
using Application.Utils;
using Domain.Context;
using Microsoft.EntityFrameworkCore;
using Processor.Consumers.IndexUser;
using Processor.Handlers.User.Create;
using Processor.Handlers.User.List;
using Serilog;

namespace FrontApiRealTime;

public class Bootstrap {
    public Bootstrap(IConfiguration configuration, IWebHostEnvironment env) {
        Configuration = configuration;
        Environment = env;

        Domain = $"https://{Configuration["Auth0:Domain"]}/";
        Log.Information($"Connect to Db Domain: {configuration.GetConnectionString("DbConnection")}");
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
            services.AddSwaggerExtension(Configuration, "Front Api Realtime Docs", "V1");
        }

        services.AddMediatR(configuration => {
            configuration.RegisterServicesFromAssemblies(Assembly.GetExecutingAssembly());
            configuration.RegisterServicesFromAssemblyContaining(typeof(ListUsersRequest));
            configuration.RegisterServicesFromAssemblyContaining(typeof(CreateUserRequest));
        });
        services.AddElasticsearch(Configuration);
        services.AddMassTransitExtension(Configuration, bus => { bus.AddConsumer<IndexUserConsumerHandler>(); });
        services.AddHealthChecks().AddCheck<GeneralHealthCheck>("front_api_real_time_service");
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
        });
        app.UseEndpoints(endpoints => { endpoints.MapControllers(); });
    }
}