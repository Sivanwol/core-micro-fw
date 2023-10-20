using Application.Extensions;
using Serilog;
namespace DevopsTools;

public class Program {
    private static string? _getCurrentEnvironment;

    static Program() {
        _getCurrentEnvironment = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");
    }

    public static int Main(string[] args) {
        try {
            SerilogExtension.ProcessLogHandler("DevopsTools", _getCurrentEnvironment ??= "development");
            Log.Information("Starting host...");
            CreateHostBuilder(args).Build().Run();
            return 0;
        }
        catch (Exception e) {
            Log.Fatal("Host terminated unexpectedly: {Message}", e.Message);
            return 1;
        }
        finally {
            Log.Information("Server Shutting down...");
            Log.CloseAndFlush();
        }
    }

    private static IHostBuilder CreateHostBuilder(string[] args) => Host
        .CreateDefaultBuilder(args)
        .ConfigureWebHostDefaults(builder => {
            builder.ConfigureAppConfiguration((context, config) => {
                var connectionString = Environment.GetEnvironmentVariable("AzureConfigConnectionString");
                var environmentLabel = Environment.GetEnvironmentVariable("EnvironmentLabel") ?? "local";
                config.AddAzureAppConfiguration(ops => {
                    ops.Connect(connectionString)
                        .Select("Devops:*", environmentLabel)
                        .ConfigureRefresh(refreshOptions => {
                            refreshOptions.Register("Devops:Reload", true).SetCacheExpiration(TimeSpan.FromSeconds(5));
                        })
                        .TrimKeyPrefix("Devops:");
                    ops.UseFeatureFlags();
                });
            });
            builder.UseStartup<Bootstrap>();
        })
        .UseSerilogExtenstion();
}