using System.Globalization;
using Application.Utils.Logs.Masking;
using Microsoft.ApplicationInsights.Extensibility;
using Serilog;
using Serilog.Core;
using Serilog.Enrichers.Sensitive;
using Serilog.Events;
using Serilog.Formatting.Compact;
namespace Application.Utils;

public static class ServicesLogger
{
    public static Logger GetLogger(string serviceName, string seqUrl, string seqApiKey)
    {
        var isAzureHost = bool.Parse(Environment.GetEnvironmentVariable("AzureHost") ?? "false");
        // 
        return !isAzureHost ? GetLocalLogger(serviceName, seqUrl, seqApiKey) : GetDeployLogger(serviceName, seqUrl, seqApiKey);
    }

    private static LoggerConfiguration GetCommonConfiguration(string serviceName)
    {
        var logger = new LoggerConfiguration()
            .MinimumLevel.Debug()
            .MinimumLevel.Override("MassTransit", LogEventLevel.Debug)
            .MinimumLevel.Override("Microsoft", LogEventLevel.Warning)
            .MinimumLevel.Override("Microsoft.EntityFrameworkCore", LogEventLevel.Warning)
            .MinimumLevel.Override("Microsoft.EntityFrameworkCore.Database.Command", LogEventLevel.Warning)
            .Enrich.FromLogContext()
            .Enrich.WithEnvironmentName()
            .Enrich.WithMachineName()
            .Enrich.WithThreadId()
            .Enrich.WithCorrelationId()
            .Enrich.WithSensitiveDataMasking(options =>
            {
                options.MaskingOperators.AddRange(new List<IMaskingOperator> {
                    new EmailMaskOperator()
                });
                options.MaskValue = "**";
            })
            .Enrich.WithProperty("ServiceName", serviceName);
        return logger;
    }

    private static Logger GetLocalLogger(string serviceName, string seqUrl, string seqApiKey)
    {
        return GetCommonConfiguration(serviceName)
            .WriteTo.Console(formatProvider: CultureInfo.InvariantCulture)
            .WriteTo.File(new CompactJsonFormatter(), Path.Combine("Logs", $"{serviceName}.service.log"),
                rollingInterval: RollingInterval.Day)
            .WriteTo.Seq(seqUrl, apiKey: seqApiKey)
            .CreateLogger();
    }
    private static Logger GetDeployLogger(string serviceName, string seqUrl, string seqApiKey)
    {
        return GetCommonConfiguration(serviceName)
            .WriteTo.Console(new RenderedCompactJsonFormatter())
            .WriteTo.ApplicationInsights(TelemetryConfiguration.CreateDefault(), TelemetryConverter.Traces)
            .WriteTo.File(new CompactJsonFormatter(),
                $@"D:\home\LogFiles\http\RawLogs\{serviceName}-log.txt",
                rollingInterval: RollingInterval.Day,
                fileSizeLimitBytes: 10 * 1024 * 1024,
                retainedFileCountLimit: 2,
                rollOnFileSizeLimit: true,
                shared: true,
                flushToDiskInterval: TimeSpan.FromSeconds(1))
            .WriteTo.Seq(seqUrl, apiKey: seqApiKey)
            .CreateLogger();

    }


    public static Logger GetLoggerTesting(string serviceName)
    {

        return new LoggerConfiguration()
            .MinimumLevel.Debug()
            .MinimumLevel.Override("MassTransit", LogEventLevel.Debug)
            .MinimumLevel.Override("Microsoft", LogEventLevel.Warning)
            .MinimumLevel.Override("Microsoft.EntityFrameworkCore", LogEventLevel.Warning)
            .MinimumLevel.Override("Microsoft.EntityFrameworkCore.Database.Command", LogEventLevel.Warning)
            .Enrich.FromLogContext()
            .WriteTo.Map(
                evt => evt.Level,
                (level, wt) =>
                {
                    wt.NUnitOutput();
                    wt.Console(formatProvider: CultureInfo.InvariantCulture);
                }).CreateLogger();
    }
}