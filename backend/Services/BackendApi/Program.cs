using Application.Extensions;
using Serilog;
namespace BackendApi;

public class Program {
    private static string? _getCurrentEnvironment;

    static Program() {
        _getCurrentEnvironment = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");
    }

    public static int Main(string[] args) {
        try {
            SerilogExtension.ProcessLogHandler("FrontApi", _getCurrentEnvironment ??= "development");
            Log.Information("Starting host...");
            CreateHostBuilder(args).Build().Run();
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