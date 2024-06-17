using Application.Utils;
using Microsoft.Extensions.Hosting;
using Serilog;
using Serilog.Events;
namespace Application.Extensions;

public static class SerilogExtension
{

    public static void ProcessLogHandler(string applicationName, string env = "development", string? seqUrl = null, string? seqApiKey = null)
    {
        Log.Logger = ServicesLogger.GetLogger(applicationName, seqUrl ?? "", seqApiKey ?? "");
        Log.Information("Logging service {ApplicationName} on {Dev} at {SeqUrl} with {SeqApiKey}", applicationName, env, seqUrl, seqApiKey);
    }
    public static IHostBuilder UseSerilogExtenstion(this IHostBuilder builder)
    {
        builder.UseSerilog(Log.Logger, true);
        return builder;
    }
}