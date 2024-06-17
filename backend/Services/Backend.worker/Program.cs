using Application.Configs;
using Application.Extensions;
using Backend.worker;
using DotEnv.Core;
using Microsoft.AspNetCore.Hosting;
using Serilog;


public class Program
{
    private static string? _getCurrentEnvironment;
    private static bool _isLocalConfiguration;
    static Program()
    {
        _getCurrentEnvironment = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");
        _isLocalConfiguration = string.IsNullOrEmpty(Environment.GetEnvironmentVariable("AzureConfigConnectionString"));
    }
    public static int Main(string[] args)
    {
        try
        {

            new EnvLoader().Load();
            var sercetSettings = new EnvBinder().Bind<BackendSecretApplicationConfig>();
            var serviceName = sercetSettings.ServiceName ?? "Backend.worker";
            var seqUrl = sercetSettings.SeqUrl ?? "";
            var seqApiKey = sercetSettings.SeqApiKey ?? "";
            SerilogExtension.ProcessLogHandler("Backend.worker", _getCurrentEnvironment ??= "development", seqUrl, seqApiKey);
            Log.Information("Starting host...");
            CreateHostBuilder(args).Build().Run();
        }
        catch (Exception e)
        {
            Log.Fatal("Host terminated unexpectedly: {Message}", e.Message);
            return 1;
        }
        finally
        {
            Log.Information("Server Shutting down...");
            Log.CloseAndFlush();
        }
        return 0;
    }
    private static IHostBuilder CreateHostBuilder(string[] args)
    {
        return Host
            .CreateDefaultBuilder(args)
            .ConfigureWebHostDefaults(builder =>
            {
                builder.ConfigureAppConfiguration((context, config) =>
                {
                    var connectionString = Environment.GetEnvironmentVariable("AzureConfigConnectionString");
                    var environmentLabel = Environment.GetEnvironmentVariable("EnvironmentLabel") ?? "local";
                    Log.Warning("Using Local Configuration");
                    if (!string.IsNullOrEmpty(connectionString))
                    {
                        Log.Warning("Switching To Azure Configuration");
                        Log.Information("Fetching Config from cloud: {ConnectionString}, {environmentLabel}", connectionString, environmentLabel);
                        config.AddAzureAppConfiguration(ops =>
                        {
                            ops.Connect(connectionString)
                                .Select("Backend:*", environmentLabel)
                                .ConfigureRefresh(refreshOptions =>
                                {
                                    refreshOptions.Register("Backend:Reload", true).SetCacheExpiration(TimeSpan.FromMinutes(30));
                                })
                                .TrimKeyPrefix("Backend:");
                            ops.UseFeatureFlags();
                            Log.Information("Connected to cloud");
                        });
                    }
                });
                Log.Information("Start Bootstrap Setup");
                builder.UseStartup<Bootstrap>();
            })
            .UseSerilogExtenstion();
    }
}