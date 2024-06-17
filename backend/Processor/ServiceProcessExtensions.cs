using Application.Configs;
using MassTransit;
using Processor.Consumers.Providers;
using Processor.Consumers.Vendors;
using Processor.StateMachine.Providers;
using Processor.StateMachine.Vendors;
using Quartz;
namespace Processor;

public static class ServiceProcessExtensions
{

    public static void AddConsumersExtension(BackendApplicationConfig configuration, IBusRegistrationConfigurator bus)
    {


        #region Providers

        bus.AddConsumer<CreateProviderInitialConsumer>();
        bus.AddConsumer<CreateProviderProcessConsumer>();
        bus.AddConsumer<CreateProviderDoneConsumer>();

        #endregion

        #region Vendors

        bus.AddConsumer<CreateVendorInitialConsumer>();
        bus.AddConsumer<CreateVendorProcessConsumer>();
        bus.AddConsumer<CreateVendorDoneConsumer>();

        #endregion

    }

    public static void AddSagaStateMachine(BackendApplicationConfig configuration, IBusRegistrationConfigurator bus)
    {
        bus.AddActivitiesFromNamespaceContaining<CreateVendorActivity>();
        bus.AddSagaStateMachine<VendorStateMachine, VendorState>(cfg =>
        {
            cfg.ConcurrentMessageLimit = 8;
        });
        bus.AddActivitiesFromNamespaceContaining<CreateProviderActivity>();
        bus.AddSagaStateMachine<ProvideStateMachine, ProviderState>(cfg =>
        {
            cfg.ConcurrentMessageLimit = 8;
        });
    }

    public static void AddJobsExtension(this IServiceCollectionQuartzConfigurator cfg, BackendApplicationConfig configuration)
    {

        #region TestJob Job Definition

        // var testJobKey = new JobKey("TestJob", "Test");
        // cfg.AddJob<TestJob>(testJobKey, j => j
        //     .WithDescription("Test Job")
        //     .WithIdentity(testJobKey));
        // cfg.AddTrigger(j => j
        //     .ForJob(testJobKey)
        //     .WithIdentity("TestJob-trigger")
        //     .WithCronSchedule("0 */2 * ? * *")); // will run every 2 minutes

        #endregion

    }
}