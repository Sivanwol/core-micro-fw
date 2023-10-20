using Serilog;
using Serilog.Core;
using Serilog.Events;
namespace Application.Utils;

public static class ServicesLogger {
    public static Logger GetLogger(string serviceName) {
        return new LoggerConfiguration()
            .MinimumLevel.Debug()
            .MinimumLevel.Override("Microsoft", LogEventLevel.Error)
            .Enrich.FromLogContext()
            .WriteTo.Map(
                evt => evt.Level,
                (level, wt) => {
                    if (level is not (LogEventLevel.Debug or LogEventLevel.Verbose or LogEventLevel.Information)) {
                        wt.File(Path.Combine("Logs", serviceName, "service.log"), rollingInterval: RollingInterval.Day);
                    }

                    wt.Console(formatProvider: System.Globalization.CultureInfo.InvariantCulture);
                }).CreateLogger();
    }


    public static Logger GetLoggerDevelopment(string serviceName) {
        return new LoggerConfiguration()
            .MinimumLevel.Debug()
            .MinimumLevel.Override("Microsoft", LogEventLevel.Debug)
            .Enrich.FromLogContext()
            .WriteTo.Map(
                evt => evt.Level,
                (level, wt) => {
                    wt.File(Path.Combine("Logs", serviceName, "service.log"), rollingInterval: RollingInterval.Day);
                    wt.Console(formatProvider: System.Globalization.CultureInfo.InvariantCulture);
                }).CreateLogger();
    }

    public static Logger GetLoggerTesting(string serviceName) {

        return new LoggerConfiguration()
            .MinimumLevel.Debug()
            .MinimumLevel.Override("Microsoft", LogEventLevel.Warning)
            .Enrich.FromLogContext()
            .WriteTo.Map(
                evt => evt.Level,
                (level, wt) => {
                    wt.NUnitOutput();
                    wt.Console(formatProvider: System.Globalization.CultureInfo.InvariantCulture);
                }).CreateLogger();
    }
}