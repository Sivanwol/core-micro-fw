using System.Reflection;
using System.Text;
using Application.Extensions;
using Domain.Context;
using Domain.Entities;
using HealthChecks.ApplicationStatus.DependencyInjection;
using HealthChecks.UI.Client;
using Infrastructure.Services.Auth.Sender;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using FluentValidation;
using Infrastructure.Requests.Processor.Countries;
using Infrastructure.Services.Auth;
using Infrastructure.Services.Auth.Models;
using Infrastructure.Validators;
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
        ActiveMigratorConnectionString =
            Configuration.GetConnectionString("MigratorDbConnection");
        Log.Information($"Connect to Db Domain: {ActiveConnectionString}");
    }

    public static IWebHostEnvironment Environment { get; set; }

    public static IConfiguration Configuration { get; set; }
    public static string ActiveConnectionString { get; private set; }
    public static string ActiveMigratorConnectionString { get; private set; }

    public void ConfigureServices(IServiceCollection services) {
        Log.Information("Start Configure Server");
        var jwtTokenConfig = Configuration.GetSection("Jwt").Get<JwtTokenConfig>();
        services.AddGenericServiceExtension(Configuration, () => {
            services.AddDbContext<DomainContext>(options => options.UseSqlServer(ActiveConnectionString));
            services.AddTransient<IDomainContext>(provider => provider.GetService<DomainContext>());
            services.Configure<SmsSettings>(c => {
                c.AccountIdentification = Configuration["SmsSettings:AccountIdentification"];
                c.AccountMessagingServiceSid = Configuration["SmsSettings:AccountMessagingServiceSid"];
                c.AccountAuthToken = Configuration["SmsSettings:AccountAuthToken"];
                c.AccountFrom = Configuration["SmsSettings:AccountFrom"];
            });
            services.Configure<EmailSettings>(c => {
                c.ApiKey = Configuration["EmailSettings:ApiKey"];
                c.SenderEmail = Configuration["EmailSettings:SenderEmail"];
                c.SenderName = Configuration["EmailSettings:SenderName"];
            });
            services.AddSingleton(jwtTokenConfig);
            services.AddTransient<IEmailSender, AuthMessageSender>();
            services.AddTransient<ISmsSender, AuthMessageSender>();
            services.AddSingleton<IJwtAuthManager, JwtAuthManager>();
            services.AddValidatorsFromAssemblyContaining<ForgetPasswordValidator>();
            services.AddValidatorsFromAssemblyContaining<RegisterUserValidator>();
            services.AddValidatorsFromAssemblyContaining<LoginUserValidator>();
            services.AddValidatorsFromAssemblyContaining<ResetPasswordValidator>();
            services.AddValidatorsFromAssemblyContaining<RefreshTokenValidator>();
            services.AddValidatorsFromAssemblyContaining<SendCodeToProviderValidator>();
            services.AddValidatorsFromAssemblyContaining<SendCodeFromProviderValidator>();

            services.AddHostedService<JwtRefreshTokenCache>();
        });
        if (Environment.IsDevelopment() || bool.Parse(Configuration["ENABLE_SWAGGER"] ?? "false")) {
            services.AddSwaggerExtension(Configuration, "Front Api Docs", "V1");
        }

        services.AddMediatR(configuration => {
            configuration.RegisterServicesFromAssemblies(Assembly.GetExecutingAssembly());
            configuration.RegisterServicesFromAssemblyContaining(typeof(ListUsersRequest));
            configuration.RegisterServicesFromAssemblyContaining<LocateCountryRequest>();
            configuration.RegisterServicesFromAssemblyContaining(typeof(CreateUserRequest));
        });
        services.AddElasticsearch(Configuration);
        services.AddMassTransitExtension(Configuration, bus => { bus.AddConsumer<IndexUserConsumerHandler>(); });
        // health checks registration 
        var DisabledHealthChecks = bool.Parse(Configuration["Disable_HealthCheck"] ?? "false");
        if (!DisabledHealthChecks) { // we check if we want the health checks to be disabled (mostly in dev)
            services.AddHealthChecks()
                .AddSqlServer(ActiveConnectionString, name: "DomainConnection", tags: new[] { "db" })
                .AddApplicationStatus();
            services.AddHealthChecksUI().AddSqlServerStorage(ActiveMigratorConnectionString);
        }

        // auth configuration
        services.Configure<IdentityOptions>(options => {
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
        // auth registration
        services.AddIdentityCore<ApplicationUser>()
            .AddRoles<IdentityRole>()
            .AddEntityFrameworkStores<DomainContext>()
            .AddSignInManager()
            .AddDefaultTokenProviders();
        services.AddAuthentication(o => {
            o.DefaultScheme = IdentityConstants.ApplicationScheme;
            o.DefaultSignInScheme = IdentityConstants.ExternalScheme;
        });

        services.AddAuthentication("Bearer").AddJwtBearer(x => {
            x.RequireHttpsMetadata = true;
            x.SaveToken = true;
            x.TokenValidationParameters = new TokenValidationParameters {
                ValidateIssuer = true,
                ValidIssuer = jwtTokenConfig.Issuer,
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(jwtTokenConfig.Secret)),
                ValidAudience = jwtTokenConfig.Audience,
                ValidateAudience = true,
                ValidateLifetime = true,
                ClockSkew = TimeSpan.FromMinutes(1)
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
            // endpoints.MapGroup("/auth").MapIdentityApi<ApplicationUser>();
            endpoints.MapControllers();
        });
    }
}