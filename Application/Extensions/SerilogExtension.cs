using Application.Utils;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Serilog;

namespace Application.Extensions; 

public static class SerilogExtension {

    public static void ProcessLogHandler(string applicationName, string env = "development") {
        switch (env.ToLower()) {
            case "development":
                Log.Logger = ServicesLogger.GetLoggerDevelopment(applicationName);
                break;
            case "test":
            case "testing":
                Log.Logger = ServicesLogger.GetLoggerTesting(applicationName);
                break;
            default:
                Log.Logger = ServicesLogger.GetLogger(applicationName);
                break;
        }
        Log.Information($"Identify environment: {env}");
    }
    public static IHostBuilder UseSerilogExtenstion(this IHostBuilder builder) {
        builder.UseSerilog(Log.Logger, true);
        return builder;
    }
}