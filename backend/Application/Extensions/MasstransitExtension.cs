using Application.Configs;
using MassTransit;
using Microsoft.Extensions.DependencyInjection;
using Quartz;
namespace Application.Extensions;

public static class MasstransitExtension {
    public static void AddMassTransitExtension(this IServiceCollection services, string connectionString, BackendApplicationConfig config,
        Action<IBusRegistrationConfigurator>? registerTransitConsumers,
        Action<IServiceCollectionQuartzConfigurator>? registerTransitScheduler = null,
        Action<IBusRegistrationConfigurator>? registerStateMachineDefinions = null) {
        services.Configure<MassTransitHostOptions>(options => {
            options.WaitUntilStarted = true;
        });
        services.AddQuartz(c => {

            c.SchedulerName = "System-Job";
            c.SchedulerId = "AUTO";
            c.UseMicrosoftDependencyInjectionJobFactory();
            c.UseDedicatedThreadPool(t => {
                t.MaxConcurrency = 10;
            });
            c.InterruptJobsOnShutdown = true;
            c.UseTimeZoneConverter();
            c.UsePersistentStore(s => {
                s.UsePostgres(connectionString);
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
            if (config.DeveloperMode) {
                bus.UsingInMemory((context, cfg) => { cfg.ConfigureEndpoints(context); });
                return;
            }
            TransitBus(config, bus, registerStateMachineDefinions);
        });
    }

    private static void TransitBus(BackendApplicationConfig configuration, IBusRegistrationConfigurator bus,
        Action<IBusRegistrationConfigurator>? registerStateMachineDefinions = null) {
        registerStateMachineDefinions?.Invoke(bus);
        bus.AddMassTransit(x => {
            x.UsingRabbitMq((context, cfg) => {
                cfg.Host(configuration.RabbitMqHost, configuration.RabbitMqVirtualHost, h => {
                    h.Username(configuration.RabbitMqUsername);
                    h.Password(configuration.RabbitMqPassword);
                    if (configuration.RabbitMqSslProtocol) {
                        h.UseSsl(ssl => {
                            ssl.Protocol = System.Security.Authentication.SslProtocols.Tls12;
                        });
                    }
                });
            });
        });
    }
}