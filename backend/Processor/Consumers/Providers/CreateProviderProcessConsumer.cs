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

public class CreateProviderProcessConsumer : BaseConsumer<ProviderCreatedProcess, ProviderState>
{

    public CreateProviderProcessConsumer(ISendEndpointProvider sendEndpointProvider, 
    IEndpointNameFormatter endpointNameFormatter) : base(sendEndpointProvider, endpointNameFormatter)
    {
    }
    public override async Task Consume(ConsumeContext<ProviderCreatedProcess> context)
    {
        LogContext.Info?.Log($"CreateProviderProcessConsumer: {context.ConversationId} - {context.Message.ProviderId} - {context.Message.ClientId ?? 0} - {context.Message.UserId}");
        // await Publish<ProviderCreatedInitial, CreateProviderActivity, ProviderCreatedArguments>(context);
    }
}
