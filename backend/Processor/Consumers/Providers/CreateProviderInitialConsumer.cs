using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MassTransit;
using MassTransit.Courier.Contracts;
using Processor.Consumers.Common;
using Processor.Contracts.Providers;
using Processor.StateMachine.Providers;

namespace Processor.Consumers.Providers;

public class CreateProviderInitialConsumer : BaseConsumer<ProviderCreatedInitial, ProviderState>
{
    private readonly ISendEndpointProvider _sendEndpointProvider;
    private IEndpointNameFormatter _endpointNameFormatter;

    public CreateProviderInitialConsumer(ISendEndpointProvider sendEndpointProvider, 
    IEndpointNameFormatter endpointNameFormatter) : base(sendEndpointProvider, endpointNameFormatter)
    {
    }
    public override async Task Consume(ConsumeContext<ProviderCreatedInitial> context)
    {
        LogContext.Info?.Log($"CreateProviderInitialConsumer: {context.ConversationId} - {context.Message.ProviderId} - {context.Message.ClientId ?? 0} - {context.Message.UserId}");
        // await Publish<ProviderCreatedInitial, CreateProviderActivity, ProviderCreatedArguments>(context);
    }
}
