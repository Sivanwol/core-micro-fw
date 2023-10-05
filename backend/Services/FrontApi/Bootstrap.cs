using System.Reflection;
using System.Text;
using Application.Extensions;
using Domain.Context;
using Domain.Entities;
using HealthChecks.ApplicationStatus.DependencyInjection;
using HealthChecks.UI.Client;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using Microsoft.IdentityModel.Tokens;
using Processor.Consumers.IndexUser;
using Processor.Handlers.User.Create;
using Processor.Handlers.User.List;
using Serilog;

namespace FrontApi;

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
        Log.Information("Start Configure Server");
        services.AddGenericServiceExtension(Configuration, () => {
            services.AddDbContext<DomainContext>(options => options.UseSqlServer(ActiveConnectionString));
            services.AddTransient<IDomainContext>(provider => provider.GetService<DomainContext>());
        });
        if (Environment.IsDevelopment() || bool.Parse(Configuration["ENABLE_SWAGGER"] ?? "false")) {
            services.AddSwaggerExtension(Configuration, "Front Api Docs", "V1");
        }

        services.AddMediatR(configuration => {
            configuration.RegisterServicesFromAssemblies(Assembly.GetExecutingAssembly());
            configuration.RegisterServicesFromAssemblyContaining(typeof(ListUsersRequest));
            configuration.RegisterServicesFromAssemblyContaining(typeof(CreateUserRequest));
        });
        services.AddElasticsearch(Configuration);
        services.AddMassTransitExtension(Configuration, bus => { bus.AddConsumer<IndexUserConsumerHandler>(); });
        // health checks registration
        services.AddHealthChecks()
            .AddSqlServer(ActiveConnectionString, name: "DomainConnection", tags: new[] { "db" })
            .AddApplicationStatus();
        services.AddHealthChecksUI().AddSqlServerStorage(ActiveConnectionString);

        // auth registration
        services.Configure<IdentityOptions>(options => {
            options.SignIn.RequireConfirmedPhoneNumber = true;
            options.User.RequireUniqueEmail = true;
            options.Password.RequireDigit = true;
            options.Password.RequireLowercase = true;
            options.Password.RequireNonAlphanumeric = true;
            options.Password.RequireUppercase = true;
            options.Password.RequiredLength = 6;
            options.Password.RequiredUniqueChars = 1;

            // Lockout settings.
            options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(5);
            options.Lockout.MaxFailedAccessAttempts = 5;
            options.Lockout.AllowedForNewUsers = true;

            // User settings.
            options.User.AllowedUserNameCharacters =
                "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789-._@+";
        });


        services.AddIdentityApiEndpoints<ApplicationUser>().AddEntityFrameworkStores<DomainContext>();
        services.AddAuthentication();

        services.AddAuthentication("Bearer").AddJwtBearer(o => {
            o.TokenValidationParameters = new TokenValidationParameters {
                ValidateIssuer = true,
                ValidateAudience = true,
                ValidateLifetime = false,
                ValidateIssuerSigningKey = true,
                ValidIssuer = Configuration["Jwt:Issuer"],
                ValidAudience = Configuration["Jwt:Audience"],
                IssuerSigningKey = new SymmetricSecurityKey
                    (Encoding.UTF8.GetBytes(Configuration["Jwt:Key"]))
            };
        });
    }

    public void Configure(IApplicationBuilder app, IWebHostEnvironment env) {
        if (Environment.IsDevelopment() || bool.Parse(Configuration["ENABLE_SWAGGER"] ?? "false")) {
            app.UseSwaggerExtension(env);
        }

        app.UseHealthChecks("/healthz", new HealthCheckOptions {
            Predicate = _ => true,
            ResponseWriter = UIResponseWriter.WriteHealthCheckUIResponse,
            ResultStatusCodes = {
                [HealthStatus.Healthy] = StatusCodes.Status200OK,
                [HealthStatus.Degraded] = StatusCodes.Status500InternalServerError,
                [HealthStatus.Unhealthy] = StatusCodes.Status503ServiceUnavailable,
            }
        });

        app.UseGenericServiceExtension(env, () => {
            if (!Environment.IsDevelopment()) {
                app.UseHsts();
            }
        });
        app.UseAuthentication();
        app.UseAuthorization();

        app.UseEndpoints(endpoints => {
            endpoints.MapGroup("/auth").MapIdentityApi<ApplicationUser>();
            endpoints.MapControllers();
        });
    }
}