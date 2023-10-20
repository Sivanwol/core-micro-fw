using Domain.Persistence.Context;
using Infrastructure.Requests.Processor.Consumers.TestConsumer;
using MassTransit;
using MediatR;
using Microsoft.Extensions.Logging;
namespace Processor.Consumers.TestConsumer;

public class TestConsumerHandler : IConsumer<TestConsumerRequest> {
    private readonly IBus _bus;
    private readonly IDomainContext _context;
    private readonly ILogger _logger;
    private readonly IMediator _mediator;
    public TestConsumerHandler(
        IMediator mediator,
        IDomainContext context,
        ILoggerFactory loggerFactory,
        IBus bus) {
        _context = context;
        _mediator = mediator;
        _bus = bus;
        _logger = loggerFactory.CreateLogger<TestConsumerHandler>();
    }

    public async Task Consume(ConsumeContext<TestConsumerRequest> context) {
        _logger.LogInformation($"Process Test Consumer - {context.Message.Message}");
    }
}

public class TestConsumerDefinition : ConsumerDefinition<TestConsumerHandler> {
    protected override void ConfigureConsumer(IReceiveEndpointConfigurator endpointConfigurator,
        IConsumerConfigurator<TestConsumerHandler> consumerConfigurator) {
        // consumerConfigurator.UseMessageRetry(retry => retry.Interval(3, TimeSpan.FromSeconds(3)));
    }
}