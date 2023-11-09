using Application.Extensions;
using Serilog;
namespace BackendApi;

public class Program {
    private static string? _getCurrentEnvironment;
    private static bool _isLocalConfiguration;
    static Program() {
        _getCurrentEnvironment = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");
        _isLocalConfiguration = string.IsNullOrEmpty(Environment.GetEnvironmentVariable("AzureConfigConnectionString")) &&
                                bool.Parse(Environment.GetEnvironmentVariable("IsLocalConfiguration") ?? "false");
    }

    public static int Main(string[] args) {
        try {
            SerilogExtension.ProcessLogHandler("Backend", _getCurrentEnvironment ??= "development");
            Log.Information("Starting host...");
            if (!_isLocalConfiguration) {
                CreateLocalConfigurationHostBuilder(args).Build().Run();
            } else {
                CreateHostBuilder(args).Build().Run();
            }
        }
        catch (Exception e) {
            Log.Fatal("Host terminated unexpectedly: {Message}", e.Message);
            return 1;
        }
        finally {
            Log.Information("Server Shutting down...");
            Log.CloseAndFlush();
        }
        return 0;
    }
    private static IHostBuilder CreateLocalConfigurationHostBuilder(string[] args) => Host
        .CreateDefaultBuilder(args)
        .ConfigureWebHostDefaults(builder => {
            builder.UseStartup<Bootstrap>();
        })
        .UseSerilogExtenstion();

    private static IHostBuilder CreateHostBuilder(string[] args) => Host
        .CreateDefaultBuilder(args)
        .ConfigureWebHostDefaults(builder => {
            builder.ConfigureAppConfiguration((context, config) => {
                var connectionString = Environment.GetEnvironmentVariable("AzureConfigConnectionString");
                var environmentLabel = Environment.GetEnvironmentVariable("EnvironmentLabel") ?? "local";
                Log.Information("Fetching Config from cloud: {ConnectionString}, {environmentLabel}", connectionString, environmentLabel);
                config.AddAzureAppConfiguration(ops => {
                    ops.Connect(connectionString)
                        .Select("Backend:*", environmentLabel)
                        .ConfigureRefresh(refreshOptions => {
                            refreshOptions.Register("Backend:Reload", true).SetCacheExpiration(TimeSpan.FromMinutes(30));
                        })
                        .TrimKeyPrefix("Backend:");
                    ops.UseFeatureFlags();
                    Log.Information("Connected to cloud");
                });
            });
            Log.Information("Start Bootstrap Setup");
            builder.UseStartup<Bootstrap>();
        })
        .UseSerilogExtenstion();
}