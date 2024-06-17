using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Application.Events;
using MassTransit;
using MassTransit.Courier.Contracts;

namespace Processor.Consumers.Common;

public abstract class BaseConsumer<T, TState> : IConsumer<T>
where T : class
where TState : EventStage
{

    private readonly ISendEndpointProvider _sendEndpointProvider;
    private IEndpointNameFormatter _endpointNameFormatter;
    public BaseConsumer(ISendEndpointProvider sendEndpointProvider, IEndpointNameFormatter endpointNameFormatter)
    {
        _sendEndpointProvider = sendEndpointProvider;
        _endpointNameFormatter = endpointNameFormatter;
    }

    public abstract Task Consume(ConsumeContext<T> context);

    protected async Task Publish<TFSource, TFActivity, TFActivityArgs>(ConsumeContext<T> context)
    where TFSource : class
    where TFActivity : class, IExecuteActivity<TFActivityArgs>
    where TFActivityArgs : class
    {

        var builder = new RoutingSlipBuilder(NewId.NextGuid());
        var processEndpoint =
            new Uri($"queue:{_endpointNameFormatter.ExecuteActivity<TFActivity, TFActivityArgs>()}");
        builder.AddActivity("Process", processEndpoint);

        var eventAddress =
            new Uri($"queue:{_endpointNameFormatter.Saga<TState>()}");
        await builder.AddSubscription(eventAddress, RoutingSlipEvents.Completed, endpoint => endpoint.Send<TFSource>(context.Message));
        await context.Execute(builder.Build());
        await context.Publish<TFSource>(context.Message, x => x.ResponseAddress = context.ResponseAddress);
    }
}
