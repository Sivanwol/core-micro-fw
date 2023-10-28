using Application.Configs;
using Application.Utils.Service;
using Infrastructure.Requests.Processor.Consumers.TestConsumer;
using MassTransit;
namespace FrontApi.HostedServices;

public class TestConsumerService : BaseSchedulerSystem {

    public TestConsumerService(IServiceScopeFactory scopeFactory) : base(scopeFactory, "*/1 * * * *") { }
    protected override async Task Process(AsyncServiceScope scope, BackendApplicationConfig config, IMessageScheduler messageScheduler, CancellationToken stoppingToken) {
        await messageScheduler.SchedulePublish(TimeSpan.FromSeconds(2), new TestConsumerRequest {
            Message = "Hello, World"
        }, stoppingToken);
    }
}