using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using MassTransit;
namespace Application.Extensions; 
public static class MasstransitExtension
{
    public static void AddMassTransitExtension(this IServiceCollection services, IConfiguration configuration, Action<IBusRegistrationConfigurator>? registerTransitConsumers ) {
        var env = configuration["ASPNETCORE_ENVIRONMENT"] ?? "development";
        var enabled = Boolean.Parse(configuration["RabbitMqSettings:Enabled"] ?? "true");
        var useRemoteHostOnDev = Boolean.Parse(configuration["RabbitMqSettings:UseRemoteHostOnDev"] ?? "true");
        services.AddMassTransit(bus => {
            bus.AddDelayedMessageScheduler();
            bus.SetSnakeCaseEndpointNameFormatter();
            registerTransitConsumers?.Invoke(bus);
            if (env == "development") {
                if (!enabled) { // Use in memory bus for development when tasker is disabled
                    bus.UsingInMemory((context, cfg) => { cfg.ConfigureEndpoints(context); });
                } else {
                    // Use local tasker for development when tasker is enabled
                    TransitRabbitMQ(useRemoteHostOnDev, configuration, bus);
                }
                return;
            }
            if (!enabled || env == "testing") { // Use in memory bus for testing when tasker is disabled
                bus.UsingInMemory((context, cfg) => { cfg.ConfigureEndpoints(context); });
                return;
            }
            TransitRabbitMQ(false, configuration, bus);
            
        });
    }
    
    private static IBusRegistrationConfigurator TransitRabbitMQ(bool useLocal, IConfiguration configuration,  IBusRegistrationConfigurator bus) {
        if (useLocal) {
            bus.UsingRabbitMq((context, cfg) => {
                cfg.Host("localhost", h => {
                    h.ConfigureBatchPublish(x => {
                        x.Enabled = true;
                        x.Timeout = TimeSpan.FromMilliseconds(2);
                    });
                });
            });
        } else {
            bus.UsingRabbitMq((context, cfg) => {
                cfg.Host(configuration["RabbitMqSettings:Host"], configuration["RabbitMqSettings:VirtualHost"], h => {
                    h.Username(configuration["RabbitMqSettings:Username"]);
                    h.Password(configuration["RabbitMqSettings:Password"]);
                });
                cfg.ConfigureEndpoints(context);
            });
        }

        return bus;
    }
}