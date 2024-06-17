using Amazon.S3;
using Application.Configs;
using Application.Configs.General;
using Application.Configs.Providers;
using Application.Constraints;
using Application.Events;
using Application.Events.Flow;
using Application.Extensions;
using Domain.Persistence;
using Domain.Persistence.Context;
using DotEnv.Core;
using Infrastructure.Services.Auth;
using Infrastructure.Services.Auth.Models;
using Infrastructure.Services.Cache;
using Infrastructure.Services.Email;
using Infrastructure.Services.S3;
using Infrastructure.Validators;
using MassTransit;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.FeatureManagement;
using Npgsql;
using OpenTelemetry.Exporter;
using OpenTelemetry.Metrics;
using OpenTelemetry.Resources;
using OpenTelemetry.Trace;
using Processor;
using Serilog;
namespace Backend.worker;

public class Bootstrap
{
    private static TimeSpan KEEP_ALIVE_INTERVAL = TimeSpan.FromMilliseconds(10000);
    private IHostApplicationLifetime _lifeTime;
    public Bootstrap(IConfiguration configuration, IWebHostEnvironment env)
    {
        Configuration = configuration;
        Environment = env;
        Log.Information("Start Bootstrap And Configuration Setup");
        ActiveAzureConfigConnectionString = Configuration["AzureConfigConnectionString"]!;
        IsLocalConfiguration = string.IsNullOrEmpty(ActiveAzureConfigConnectionString);
        ActiveConnectionString = Configuration.GetConnectionString("DbConnection")!;
    }
    public static IWebHostEnvironment Environment { get; set; }
    public static bool IsLocalConfiguration { get; set; }

    public static IConfiguration Configuration { get; set; }
    public static string ActiveAzureConfigConnectionString { get; private set; }
    public static string ActiveConnectionString { get; set; }

    public async void ConfigureServices(IServiceCollection services)
    {
        Log.Information("Start Configure Server");
        // Load configuration from Azure App Configuration
        services.AddAzureAppConfiguration()
            .AddFeatureManagement(Configuration.GetSection("FeatureManagement"));
        if (!IsLocalConfiguration)
        {
            services.Configure<BackendApplicationConfig>(Configuration.GetSection("ApplicationConfig"));
        }
        AppContext.SetSwitch("Npgsql.EnableLegacyTimestampBehavior", true);
        services.Configure<MailServerConfig>(Configuration.GetSection("MailConfig"));
        var applicationConfig = Configuration.GetSection("ApplicationConfig").Get<BackendApplicationConfig>()!;
        new EnvLoader().Load();

        var contentPathRoot = Environment.ContentRootPath;
        var uploadFolder = Path.Combine(contentPathRoot, "UploadedFiles");
        applicationConfig.UploadFolder = uploadFolder;
        var secretSettings = new EnvBinder().Bind<BackendSecretApplicationConfig>();
        var seqApiKey = "";
        if (secretSettings != null)
        {
            Log.Information("Override Configuration base environment variables");
            applicationConfig.JwtSecret = secretSettings.JwtSecret;
            applicationConfig.SMSProviderInfouToken = secretSettings.SMSProviderInfouToken;
            applicationConfig.EMailPassword = secretSettings.EMailPassword;
            applicationConfig.EMailUser = secretSettings.EMailUser;
            ActiveConnectionString = secretSettings.DatabaseConnectionString;
            seqApiKey = secretSettings.SeqApiKey;
            Log.Information("Change Domain Connection: {ActiveConnectionString}", ActiveConnectionString);
        }
        else
        {
            Log.Logger.Fatal("Secret Settings are not loaded, please check the environment variables");
            _lifeTime.StopApplication();
            // we need to return explicit after exit
            return;
        }
        var jwtTokenConfig = new JwtTokenConfig
        {
            Secret = applicationConfig.JwtSecret,
            Issuer = applicationConfig.JwtIssuer,
            Audience = applicationConfig.JwtAudience,
            AccessTokenExpiration = applicationConfig.JwtAccessTokenExpired,
            RefreshTokenExpiration = applicationConfig.JwtRefreshTokenExpired
        };
        var infoUSmsProvider = new InfoUSmsConfig
        {
            UserName = applicationConfig.SMSProviderInfouUserName,
            Token = applicationConfig.SMSProviderInfouToken,
            Sender = applicationConfig.SMSProviderInfouSender
        };
        #region Aws Config

        services.AddDefaultAWSOptions(Configuration.GetAWSOptions());
        services.AddAWSService<IAmazonS3>();

        #endregion

        #region Open Telemetry Config

        if (secretSettings.EnableOpenTelemetry)
        { // if we want to enabled this only then it will be per config
            services.AddOpenTelemetry()
                .ConfigureResource(r =>
                {
                    r.AddService(secretSettings.ServiceName);
                    r.AddAttributes(new List<KeyValuePair<string, object>> {
                        new KeyValuePair<string, object>("Environment", Environment.EnvironmentName),
                        new KeyValuePair<string, object>("Version", applicationConfig.APIMajorVersion),
                        new KeyValuePair<string, object>("MaintenanceMode", applicationConfig.MaintenanceMode),
                        new KeyValuePair<string, object>("EnableCache", applicationConfig.EnableCache),
                        new KeyValuePair<string, object>("EnableGraphQLCache", applicationConfig.EnableGraphQLCache),
                        new KeyValuePair<string, object>("DeveloperModeEnabledOTP", applicationConfig.DeveloperModeEnabledOTP),
                        new KeyValuePair<string, object>("EnableHealthCheck", !applicationConfig.DisableHealthCheck),
                        new KeyValuePair<string, object>("DeveloperMode", applicationConfig.DeveloperMode),
                        new KeyValuePair<string, object>("EnableSwagger", applicationConfig.EnableSwagger)
                    });
                    r.AddEnvironmentVariableDetector();
                })
                .WithTracing(tracing =>
                {
                    tracing.AddAspNetCoreInstrumentation();
                    // tracing.AddHttpClientInstrumentation();
                    tracing.AddNpgsql();
                    tracing.AddSource("MassTransit");
                    tracing.AddQuartzInstrumentation();
                    tracing.AddSqlClientInstrumentation(options =>
                    {
                        options.SetDbStatementForText = true;
                        options.SetDbStatementForStoredProcedure = true;
                        options.EnableConnectionLevelAttributes = true;
                        options.RecordException = true;
                    });
                    // tracing.AddGrpcClientInstrumentation();
                    // tracing for seq exporter
                    tracing.AddOtlpExporter(opt =>
                    {
                        opt.Endpoint = new Uri($"{secretSettings.SeqUrl}/ingest/otlp/v1/traces");
                        opt.Protocol = OtlpExportProtocol.HttpProtobuf;
                        opt.Headers = $"X-Seq-ApiKey={seqApiKey}";
                    });
                    // tracing for jaeger exporter
                    // tracing.AddJaegerExporter(o =>{
                    //     o.AgentHost = HostMetadataCache.IsRunningInContainer ? "jaeger" : "localhost";
                    //     o.AgentPort = 6831;
                    //     o.MaxPayloadSizeInBytes = 4096;
                    //     o.ExportProcessorType = ExportProcessorType.Batch;
                    //     o.BatchExportProcessorOptions = new BatchExportProcessorOptions<Activity>
                    //     {
                    //         MaxQueueSize = 2048,
                    //         ScheduledDelayMilliseconds = 5000,
                    //         ExporterTimeoutMilliseconds = 30000,
                    //         MaxExportBatchSize = 512,
                    //     };
                    // });
                    tracing.AddConsoleExporter();
                })
                .WithMetrics(metrics =>
                {
                    metrics.AddAspNetCoreInstrumentation();
                    metrics.AddHttpClientInstrumentation();
                    metrics.AddOtlpExporter(opt =>
                    {
                        opt.Endpoint = new Uri($"{secretSettings.SeqUrl}/ingest/otlp/v1/metrics");
                        opt.Protocol = OtlpExportProtocol.HttpProtobuf;
                        opt.Headers = $"X-Seq-ApiKey={seqApiKey}";
                    });
                    metrics.AddConsoleExporter();
                });
        }

        #endregion

        services.AddSingleton(jwtTokenConfig!);
        services.AddSingleton(applicationConfig);
        services.AddSingleton(infoUSmsProvider);
        // auth configuration


        services.AddDbContext<DomainContext>(options =>
        {
            options.UseNpgsql(ActiveConnectionString)
            .UseSnakeCaseNamingConvention()
                .LogTo(Console.WriteLine, LogLevel.Information);
            if (Environment.IsDevelopment())
            {
                options.EnableSensitiveDataLogging()
                    .EnableDetailedErrors();
            }
        });
        services.Configure<FrontEndPaths>(Configuration.GetSection("FrontEndWebPaths"));

        services.AddTransient<IDomainContext>(provider => provider.GetService<DomainContext>()!);
        services.AddTransient<ITemplateService, TemplateService>();
        services.AddTransient<IMailService, MailService>();
        services.AddSingleton<IJwtAuthManager, JwtAuthManager>();
        services.AddTransient<ICacheService, CacheService>();
        services.AddTransient<IStorageControlService, StorageControlService>();
        services.AddValidatorsExtension();
        services.AddRazorPages();

        // services.AddLZ4Compressor("lz4");
        // More info: https://easycaching.readthedocs.io/en/latest/Redis/
        services.AddEasyCaching(option =>
        {
            option.UseRedis(config =>
            {

                config.DBConfig.AllowAdmin = true;
                config.DBConfig.SyncTimeout = 10000;
                config.DBConfig.AsyncTimeout = 10000;
                config.DBConfig.Endpoints.Add(new EasyCaching.Core.Configurations.ServerEndPoint(applicationConfig.RedisHost, applicationConfig.RedisPort));
                if (applicationConfig.RedisAuth.Trim() != "")
                {
                    config.DBConfig.Password = applicationConfig.RedisAuth;
                }
                if (applicationConfig.RabbitMqSslProtocol)
                {
                    config.DBConfig.IsSsl = true;
                }
                config.DBConfig.Database = applicationConfig.RedisDb;
                config.EnableLogging = true;
                config.DBConfig.ConnectionTimeout = 10000;
                config.SerializerName = Cache.SerializerName;
            }, Cache.ProviderName)
                .WithJson()
                .WithMessagePack(Cache.SerializerName);
            // .WithCompressor(Cache.SerializerName, "lz4");
        });

        // add repositories
        services.AddRepositoriesExtension();
        // add mocks services
        services.AddMocksExtension();

        #region AddMediatR

        services.AddMediatR(cfg =>
            cfg.RegisterServicesFromAssemblies(AppDomain.CurrentDomain.GetAssemblies().Where(x => !x.FullName!.Contains("Sql")).ToArray()));
        // enabled elastic search 
        // services.AddElasticsearch(Configuration);
        services.AddMassTransitExtension(ActiveConnectionString, applicationConfig, bus =>
        {
            ServiceProcessExtensions.AddConsumersExtension(applicationConfig, bus);
        }, cfg =>
        {
            cfg.AddJobsExtension(applicationConfig);
        }, bus =>
        {
            ServiceProcessExtensions.AddSagaStateMachine(applicationConfig, bus);
        });

        #endregion
        // // health checks registration 
        // if (!applicationConfig.DisableHealthCheck) { // we check if we want the health checks to be disabled (mostly in dev)
        //     var redisConnectionString = applicationConfig.RedisSsl
        //         ? $"rediss://{applicationConfig.RedisHost}:{applicationConfig.RedisPort},defaultDatabase={applicationConfig.RedisDb}"
        //         : $"redis://{applicationConfig.RedisHost}:{applicationConfig.RedisPort},defaultDatabase={applicationConfig.RedisDb}";
        //     if (applicationConfig.RedisAuth.Trim() == "") {
        //         redisConnectionString += $",password={applicationConfig.RedisAuth}";
        //     }
        //     var rabbitMqConnectionString = applicationConfig.RabbitMqSslProtocol
        //         ? $"amqps://{applicationConfig.RabbitMqUsername}:{applicationConfig.RabbitMqPassword}@{applicationConfig.RabbitMqHost}"
        //         : $"amqp://{applicationConfig.RabbitMqUsername}:{applicationConfig.RabbitMqPassword}@{applicationConfig.RabbitMqHost}";
        //     services.AddHealthChecks()
        //         // .AddMySql(ActiveConnectionString, tags: new[] {
        //         //     "db"
        //         // })
        //         .AddRedis(redisConnectionString, tags: new[] {
        //             "redis"
        //         })
        //         .AddRabbitMQ(rabbitConnectionString: rabbitMqConnectionString, tags: new[] {
        //             "rabbitmq"
        //         })
        //         .AddApplicationStatus();
        //     services
        //         .AddHealthChecksUI()
        //         .AddMySqlStorage(ActiveConnectionString);
        // }
        // auth registration
    }
    public void Configure(IApplicationBuilder app, IWebHostEnvironment env, IHostApplicationLifetime lifetime)
    {
        Log.Information("Perp Server");
        _lifeTime = lifetime;
        var applicationConfig = Configuration.GetSection("ApplicationConfig").Get<BackendApplicationConfig>()!;


        // if (!IsLocalConfiguration)
        // {
        //     app.UseAzureAppConfiguration();
        // }

        Log.Information("End Perp Setup Starting Server");
    }
}