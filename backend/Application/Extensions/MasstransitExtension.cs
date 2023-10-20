using Application.Configs;
using MassTransit;
using Microsoft.Extensions.DependencyInjection;
using Quartz;
namespace Application.Extensions;

public static class MasstransitExtension {
    public static void AddMassTransitExtension(this IServiceCollection services, BackendApplicationConfig config, Action<IBusRegistrationConfigurator>? registerTransitConsumers,
        Action<IServiceCollectionQuartzConfigurator>? registerTransitScheduler = null) {
        services.Configure<MassTransitHostOptions>(options => {
            options.WaitUntilStarted = true;
        });
        services.AddQuartz(c => {
            c.SchedulerName = "Mylo-System-Job";
            c.SchedulerId = "AUTO";
            c.UseMicrosoftDependencyInjectionJobFactory();
            c.UseDedicatedThreadPool(t => {
                t.MaxConcurrency = 10;
            });
            c.InterruptJobsOnShutdown = true;
            c.UseTimeZoneConverter();
            c.UsePersistentStore(s => {
                s.UseSqlServer(config.ConnectionString);
                s.UseProperties = true;
                s.RetryInterval = TimeSpan.FromSeconds(15);
                s.UseJsonSerializer();
                s.UseClustering(c => {
                    c.CheckinMisfireThreshold = TimeSpan.FromSeconds(20);
                    c.CheckinInterval = TimeSpan.FromSeconds(10);
                });

            });
            registerTransitScheduler?.Invoke(c);
        });
        services.AddQuartzHostedService(options => {
            options.StartDelay = TimeSpan.FromSeconds(5);
            options.WaitForJobsToComplete = true;
            options.AwaitApplicationStarted = true;
        });
        services.AddMassTransit(bus => {
            bus.AddServiceBusMessageScheduler();
            // bus.AddPublishMessageScheduler();
            bus.SetSnakeCaseEndpointNameFormatter();
            bus.AddQuartzConsumers();
            registerTransitConsumers?.Invoke(bus);
            if (config.IsTesting) {
                bus.UsingInMemory((context, cfg) => { cfg.ConfigureEndpoints(context); });
                return;
            }
            TransitBus(config, bus);
        });
    }

    private static void TransitBus(BackendApplicationConfig configuration, IBusRegistrationConfigurator bus) {
        bus.UsingAzureServiceBus((context, cfg) => {
            cfg.Host(configuration.AzureServiceBus);
            cfg.EnablePartitioning = true;
            cfg.EnableDeadLetteringOnMessageExpiration = true;
            cfg.UseServiceBusMessageScheduler();
            // cfg.UsePublishMessageScheduler();
            cfg.ConfigureEndpoints(context);
        });
    }
}