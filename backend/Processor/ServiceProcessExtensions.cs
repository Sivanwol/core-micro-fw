using Application.Configs;
using MassTransit;
using Quartz;
namespace Processor;

public static class ServiceProcessExtensions {

    public static void AddConsumersExtension(BackendApplicationConfig configuration, IBusRegistrationConfigurator bus) {

        #region Consumers Registers

        // bus.AddConsumer<TestConsumerHandler>();

        #endregion

    }

    public static void AddJobsExtension(this IServiceCollectionQuartzConfigurator cfg, BackendApplicationConfig configuration) {

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